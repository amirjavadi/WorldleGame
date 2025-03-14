using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;

namespace WordleBackend.Services
{
    public interface IScoreService
    {
        Task<Score> AddScoreAsync(int userId, string difficulty, int attempts, double guessTime);
        Task<List<Score>> GetUserScoresAsync(int userId, int page = 1, int pageSize = 10);
        Task<List<Score>> GetTopScoresAsync(string period = "all", int count = 10);
        Task<int> GetUserRankAsync(int userId, string period = "all");
    }

    public class ScoreService : IScoreService
    {
        private readonly AppDbContext _context;

        public ScoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Score> AddScoreAsync(int userId, string difficulty, int attempts, double guessTime)
        {
            var score = new Score
            {
                UserId = userId,
                GameDifficulty = difficulty,
                Attempts = attempts,
                GuessTime = guessTime,
                Points = CalculateBasePoints(difficulty, attempts),
                SpeedBonus = CalculateSpeedBonus(guessTime),
                DifficultyBonus = CalculateDifficultyBonus(difficulty),
                StreakBonus = await CalculateStreakBonusAsync(userId)
            };

            _context.Scores.Add(score);

            // Update user stats
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.TotalScore += score.Value;
                user.TotalGames++;
                if (attempts > 0 && attempts <= 6)
                {
                    user.WonGames++;
                    user.CurrentStreak++;
                    user.MaxStreak = Math.Max(user.MaxStreak, user.CurrentStreak);
                }
                else
                {
                    user.CurrentStreak = 0;
                }
            }

            await _context.SaveChangesAsync();
            return score;
        }

        public async Task<List<Score>> GetUserScoresAsync(int userId, int page = 1, int pageSize = 10)
        {
            return await _context.Scores
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Score>> GetTopScoresAsync(string period = "all", int count = 10)
        {
            var query = _context.Scores.AsQueryable();

            // Filter by period
            query = period switch
            {
                "daily" => query.Where(s => s.CreatedAt.Date == DateTime.UtcNow.Date),
                "weekly" => query.Where(s => s.CreatedAt >= DateTime.UtcNow.AddDays(-7)),
                "monthly" => query.Where(s => s.CreatedAt >= DateTime.UtcNow.AddMonths(-1)),
                _ => query
            };

            return await query
                .OrderByDescending(s => s.Value)
                .Take(count)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<int> GetUserRankAsync(int userId, string period = "all")
        {
            var query = _context.Scores.AsQueryable();

            // Filter by period
            query = period switch
            {
                "daily" => query.Where(s => s.CreatedAt.Date == DateTime.UtcNow.Date),
                "weekly" => query.Where(s => s.CreatedAt >= DateTime.UtcNow.AddDays(-7)),
                "monthly" => query.Where(s => s.CreatedAt >= DateTime.UtcNow.AddMonths(-1)),
                _ => query
            };

            var userScore = await query
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.Value)
                .FirstOrDefaultAsync();

            if (userScore == null)
                return 0;

            return await query
                .CountAsync(s => s.Value > userScore.Value) + 1;
        }

        private int CalculateBasePoints(string difficulty, int attempts)
        {
            var basePoints = difficulty switch
            {
                "easy" => 100,
                "medium" => 200,
                "hard" => 300,
                _ => 100
            };

            // کم کردن امتیاز بر اساس تعداد حدس‌ها
            return basePoints - ((attempts - 1) * 10);
        }

        private int CalculateSpeedBonus(double guessTime)
        {
            if (guessTime <= 30) return 50;  // کمتر از 30 ثانیه
            if (guessTime <= 60) return 30;  // کمتر از 1 دقیقه
            if (guessTime <= 120) return 10; // کمتر از 2 دقیقه
            return 0;
        }

        private int CalculateDifficultyBonus(string difficulty)
        {
            return difficulty switch
            {
                "hard" => 50,
                "medium" => 25,
                "easy" => 0,
                _ => 0
            };
        }

        private async Task<int> CalculateStreakBonusAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return 0;

            return user.CurrentStreak switch
            {
                >= 10 => 100, // 10 برد متوالی یا بیشتر
                >= 5 => 50,  // 5 برد متوالی یا بیشتر
                >= 3 => 25,  // 3 برد متوالی یا بیشتر
                _ => 0
            };
        }
    }
} 