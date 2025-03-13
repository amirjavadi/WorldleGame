using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;

namespace WordleBackend.Services
{
    public interface IGameService
    {
        Task<Word> GetTodaysWordAsync();
        Task<(bool isCorrect, string[] feedback)> CheckGuessAsync(string userId, string guess);
        Task<GameHistory> SubmitGuessAsync(string userId, string guess);
        Task<GameHistory> GetGameHistoryAsync(string userId, DateTime date);
        Task<IEnumerable<GameHistory>> GetUserGameHistoryAsync(string userId, int page = 1, int pageSize = 10);
    }

    public class GameService : IGameService
    {
        private readonly AppDbContext _context;

        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Word> GetTodaysWordAsync()
        {
            var today = DateTime.UtcNow.Date;
            var word = await _context.Words
                .FirstOrDefaultAsync(w => w.Date.Date == today && w.IsActive);

            if (word == null)
            {
                throw new Exception("کلمه امروز هنوز تنظیم نشده است");
            }

            return word;
        }

        public async Task<(bool isCorrect, string[] feedback)> CheckGuessAsync(string userId, string guess)
        {
            var todaysWord = await GetTodaysWordAsync();
            var correctWord = todaysWord.Text;

            if (guess.Length != correctWord.Length)
            {
                throw new Exception("طول کلمه حدس زده شده باید 5 حرف باشد");
            }

            var feedback = new string[5];
            var usedPositions = new bool[5];

            // اول حروف درست در جای درست را مشخص می‌کنیم
            for (int i = 0; i < 5; i++)
            {
                if (guess[i] == correctWord[i])
                {
                    feedback[i] = "سبز";
                    usedPositions[i] = true;
                }
            }

            // سپس حروف درست در جای نادرست را مشخص می‌کنیم
            for (int i = 0; i < 5; i++)
            {
                if (feedback[i] != null) continue;

                var found = false;
                for (int j = 0; j < 5; j++)
                {
                    if (!usedPositions[j] && guess[i] == correctWord[j])
                    {
                        feedback[i] = "زرد";
                        usedPositions[j] = true;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    feedback[i] = "خاکستری";
                }
            }

            return (guess == correctWord, feedback);
        }

        public async Task<GameHistory> SubmitGuessAsync(string userId, string guess)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("کاربر یافت نشد");
            }

            var todaysWord = await GetTodaysWordAsync();
            var (isCorrect, feedback) = await CheckGuessAsync(userId, guess);

            var today = DateTime.UtcNow.Date;
            var gameHistory = await _context.GameHistories
                .FirstOrDefaultAsync(gh => gh.UserId == userId && gh.PlayedAt.Date == today);

            if (gameHistory == null)
            {
                gameHistory = new GameHistory
                {
                    UserId = userId,
                    WordId = todaysWord.Id,
                    Attempts = 1,
                    IsWon = isCorrect,
                    Guesses = JsonSerializer.Serialize(new[] { guess }),
                    Score = CalculateScore(1, isCorrect)
                };
                _context.GameHistories.Add(gameHistory);
            }
            else
            {
                if (gameHistory.IsWon || gameHistory.Attempts >= 6)
                {
                    throw new Exception("بازی امروز به پایان رسیده است");
                }

                var guesses = JsonSerializer.Deserialize<string[]>(gameHistory.Guesses)?.ToList() ?? new List<string>();
                guesses.Add(guess);
                gameHistory.Guesses = JsonSerializer.Serialize(guesses);
                gameHistory.Attempts++;
                gameHistory.IsWon = isCorrect;
                gameHistory.Score = CalculateScore(gameHistory.Attempts, isCorrect);
            }

            // به‌روزرسانی آمار کاربر
            user.TotalGames = gameHistory.Attempts == 1 ? user.TotalGames + 1 : user.TotalGames;
            if (isCorrect)
            {
                user.WonGames++;
                user.CurrentStreak++;
                user.MaxStreak = Math.Max(user.MaxStreak, user.CurrentStreak);
            }
            else if (gameHistory.Attempts >= 6)
            {
                user.CurrentStreak = 0;
            }

            await _context.SaveChangesAsync();
            return gameHistory;
        }

        public async Task<GameHistory> GetGameHistoryAsync(string userId, DateTime date)
        {
            return await _context.GameHistories
                .Include(gh => gh.Word)
                .FirstOrDefaultAsync(gh => gh.UserId == userId && gh.PlayedAt.Date == date.Date);
        }

        public async Task<IEnumerable<GameHistory>> GetUserGameHistoryAsync(string userId, int page = 1, int pageSize = 10)
        {
            return await _context.GameHistories
                .Include(gh => gh.Word)
                .Where(gh => gh.UserId == userId)
                .OrderByDescending(gh => gh.PlayedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private int CalculateScore(int attempts, bool isWon)
        {
            if (!isWon) return 0;
            // امتیاز بر اساس تعداد تلاش‌ها: 6 تلاش = 100 امتیاز، 1 تلاش = 600 امتیاز
            return (7 - attempts) * 100;
        }
    }
} 