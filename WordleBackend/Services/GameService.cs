using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;

namespace WordleBackend.Services
{
    public interface IGameService
    {
        Task<GameHistory> StartNewGameAsync(int userId);
        Task<GameHistory> MakeGuessAsync(int gameId, int userId, string guess);
        Task<List<GameHistory>> GetUserGameHistoryAsync(int userId, int page = 1, int pageSize = 10);
        Task<GameHistory> GetGameHistoryAsync(int userId, DateTime date);
        Task<List<GameHistory>> GetActiveGamesAsync(int userId);
        Task<List<GameHistory>> GetCompletedGamesAsync(int userId);
    }

    public class GameService : IGameService
    {
        private readonly AppDbContext _context;
        private readonly IWordService _wordService;
        private const int MaxAttempts = 6;

        public GameService(AppDbContext context, IWordService wordService)
        {
            _context = context;
            _wordService = wordService;
        }

        public async Task<GameHistory> StartNewGameAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var word = await _wordService.GetRandomWordAsync();
            if (word == null)
                throw new Exception("No words available");

            var gameHistory = new GameHistory
            {
                UserId = userId,
                WordId = word.Id,
                StartTime = DateTime.UtcNow,
                Status = "InProgress",
                Attempts = 0,
                Score = 0,
                Guesses = "",
                LastGuess = ""
            };

            _context.GameHistories.Add(gameHistory);
            await _context.SaveChangesAsync();

            return gameHistory;
        }

        public async Task<GameHistory> MakeGuessAsync(int gameId, int userId, string guess)
        {
            var gameHistory = await _context.GameHistories
                .Include(gh => gh.Word)
                .FirstOrDefaultAsync(gh => gh.Id == gameId && gh.UserId == userId);

            if (gameHistory == null)
                throw new Exception("Game not found");

            if (gameHistory.Status != "InProgress")
                throw new Exception("Game is already completed");

            if (gameHistory.Attempts >= MaxAttempts)
                throw new Exception("Maximum attempts reached");

            if (guess.Length != gameHistory.Word.Text.Length)
                throw new Exception($"Guess must be {gameHistory.Word.Text.Length} characters long");

            gameHistory.Attempts++;
            gameHistory.LastGuess = guess;
            gameHistory.Guesses = string.IsNullOrEmpty(gameHistory.Guesses)
                ? guess
                : $"{gameHistory.Guesses},{guess}";

            if (guess.Equals(gameHistory.Word.Text, StringComparison.OrdinalIgnoreCase))
            {
                gameHistory.Status = "Won";
                gameHistory.EndTime = DateTime.UtcNow;
                gameHistory.Score = CalculateScore(gameHistory.Attempts);
            }
            else if (gameHistory.Attempts >= MaxAttempts)
            {
                gameHistory.Status = "Lost";
                gameHistory.EndTime = DateTime.UtcNow;
                gameHistory.Score = 0;
            }

            await _context.SaveChangesAsync();
            return gameHistory;
        }

        public async Task<List<GameHistory>> GetUserGameHistoryAsync(int userId, int page = 1, int pageSize = 10)
        {
            return await _context.GameHistories
                .Include(gh => gh.Word)
                .Where(gh => gh.UserId == userId)
                .OrderByDescending(gh => gh.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<GameHistory> GetGameHistoryAsync(int userId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _context.GameHistories
                .Include(gh => gh.Word)
                .Where(gh => gh.UserId == userId && gh.StartTime >= startOfDay && gh.StartTime < endOfDay)
                .OrderByDescending(gh => gh.StartTime)
                .FirstOrDefaultAsync();
        }

        public async Task<List<GameHistory>> GetActiveGamesAsync(int userId)
        {
            return await _context.GameHistories
                .Include(gh => gh.Word)
                .Where(gh => gh.UserId == userId && gh.Status == "InProgress")
                .OrderByDescending(gh => gh.StartTime)
                .ToListAsync();
        }

        public async Task<List<GameHistory>> GetCompletedGamesAsync(int userId)
        {
            return await _context.GameHistories
                .Include(gh => gh.Word)
                .Where(gh => gh.UserId == userId && (gh.Status == "Won" || gh.Status == "Lost"))
                .OrderByDescending(gh => gh.StartTime)
                .ToListAsync();
        }

        private int CalculateScore(int attempts)
        {
            // Score calculation based on attempts:
            // 1 attempt: 100 points
            // 2 attempts: 80 points
            // 3 attempts: 60 points
            // 4 attempts: 40 points
            // 5 attempts: 20 points
            // 6 attempts: 10 points
            return Math.Max(0, 120 - (attempts * 20));
        }
    }
} 