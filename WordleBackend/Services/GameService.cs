using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;
using WordleBackend.Services.Interfaces;

namespace WordleBackend.Services
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _context;
        private readonly IWordService _wordService;
        private readonly ILeaderboardService _leaderboardService;
        private const int MaxAttempts = 6;

        public GameService(AppDbContext context, IWordService wordService, ILeaderboardService leaderboardService)
        {
            _context = context;
            _wordService = wordService;
            _leaderboardService = leaderboardService;
        }

        public async Task<GameHistory> StartNewGameAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("کاربر یافت نشد");

            var word = await _wordService.GetRandomWordAsync();
            if (word == null)
                throw new InvalidOperationException("کلمه‌ای برای بازی یافت نشد");

            var gameHistory = new GameHistory
            {
                UserId = userId,
                WordId = word.Id,
                StartTime = DateTime.UtcNow,
                Status = "in_progress",
                Attempts = 0,
                Score = 0,
                Difficulty = word.Difficulty,
                Guesses = new List<string>()
            };

            _context.GameHistories.Add(gameHistory);
            await _context.SaveChangesAsync();

            return gameHistory;
        }

        public async Task<GameHistory> MakeGuessAsync(int gameId, string guess)
        {
            var game = await _context.GameHistories
                .Include(g => g.Word)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                throw new InvalidOperationException("بازی یافت نشد");

            if (game.Status != "in_progress")
                throw new InvalidOperationException("بازی به پایان رسیده است");

            if (game.Attempts >= MaxAttempts)
                throw new InvalidOperationException("تعداد تلاش‌های مجاز به پایان رسیده است");

            if (guess.Length != game.Word.Text.Length)
                throw new InvalidOperationException($"کلمه باید {game.Word.Text.Length} حرف باشد");

            game.Attempts++;
            game.Guesses.Add(guess);
            game.LastGuess = guess;

            if (guess.Equals(game.Word.Text, StringComparison.OrdinalIgnoreCase))
            {
                game.Status = "won";
                game.EndTime = DateTime.UtcNow;
                game.Score = CalculateScore(game.Attempts, game.Difficulty);
            }
            else if (game.Attempts >= MaxAttempts)
            {
                game.Status = "lost";
                game.EndTime = DateTime.UtcNow;
                game.Score = 0;
            }

            await _context.SaveChangesAsync();

            // Update leaderboard if game is finished
            if (game.Status != "in_progress")
            {
                await _leaderboardService.UpdateLeaderboardAsync(game);
            }

            return game;
        }

        private int CalculateScore(int attempts, string difficulty)
        {
            var baseScore = 1000;
            var attemptPenalty = (attempts - 1) * 100;
            var difficultyMultiplier = difficulty.ToLower() switch
            {
                "easy" => 1.0,
                "medium" => 1.5,
                "hard" => 2.0,
                _ => 1.0
            };

            var finalScore = (int)((baseScore - attemptPenalty) * difficultyMultiplier);
            return Math.Max(0, finalScore);
        }

        public async Task<IEnumerable<GameHistory>> GetUserGameHistoryAsync(int userId)
        {
            return await _context.GameHistories
                .Include(g => g.Word)
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameHistory>> GetGameHistoryAsync()
        {
            return await _context.GameHistories
                .Include(g => g.Word)
                .Include(g => g.User)
                .OrderByDescending(g => g.StartTime)
                .ToListAsync();
        }

        public async Task<List<GameHistory>> GetActiveGamesAsync(int userId)
        {
            return await _context.GameHistories
                .Include(gh => gh.Word)
                .Where(gh => gh.UserId == userId && gh.Status == "in_progress")
                .OrderByDescending(gh => gh.StartTime)
                .ToListAsync();
        }

        public async Task<List<GameHistory>> GetCompletedGamesAsync(int userId)
        {
            return await _context.GameHistories
                .Include(gh => gh.Word)
                .Where(gh => gh.UserId == userId && (gh.Status == "won" || gh.Status == "lost"))
                .OrderByDescending(gh => gh.StartTime)
                .ToListAsync();
        }
    }
} 