using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;

namespace WordleBackend.Services
{
    public interface IUserProfileService
    {
        Task<User> GetUserProfileAsync(int userId);
        Task<User> UpdateUserProfileAsync(int userId, User updatedProfile);
        Task<UserStats> GetUserStatsAsync(int userId);
        Task<List<GameHistory>> GetUserGameHistoryAsync(int userId, int page = 1, int pageSize = 10);
        Task UpdateUserSettingsAsync(int userId, UserSettings settings);
    }

    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext _context;

        public UserProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserProfileAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.GameHistories)
                .Include(u => u.Scores)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("کاربر یافت نشد");

            return user;
        }

        public async Task<User> UpdateUserProfileAsync(int userId, User updatedProfile)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("کاربر یافت نشد");

            // به‌روزرسانی فیلدهای قابل ویرایش
            user.DisplayName = updatedProfile.DisplayName;
            user.Avatar = updatedProfile.Avatar;
            user.Bio = updatedProfile.Bio;
            user.Email = updatedProfile.Email;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserStats> GetUserStatsAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.GameHistories)
                .Include(u => u.Scores)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("کاربر یافت نشد");

            var wonGames = user.GameHistories.Count(gh => gh.Status == "Won");
            var totalGames = user.GameHistories.Count;

            return new UserStats
            {
                TotalGames = totalGames,
                WonGames = wonGames,
                WinRate = totalGames > 0 ? (double)wonGames / totalGames * 100 : 0,
                CurrentStreak = user.CurrentStreak,
                BestStreak = user.MaxStreak,
                AverageGuesses = CalculateAverageGuesses(user.GameHistories),
                TotalScore = user.TotalScore,
                RecentScores = user.Scores.OrderByDescending(s => s.CreatedAt).Take(5).ToList()
            };
        }

        private double CalculateAverageGuesses(ICollection<GameHistory> gameHistories)
        {
            if (gameHistories == null || !gameHistories.Any())
                return 0;

            var wonGames = gameHistories.Where(gh => gh.Status == "Won");
            if (!wonGames.Any())
                return 0;

            return wonGames.Average(g => g.Attempts);
        }

        public async Task<List<GameHistory>> GetUserGameHistoryAsync(int userId, int page = 1, int pageSize = 10)
        {
            return await _context.GameHistories
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateUserSettingsAsync(int userId, UserSettings settings)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("کاربر یافت نشد");

            user.PreferredLanguage = settings.PreferredLanguage;
            user.Theme = settings.Theme;
            user.SoundEnabled = settings.SoundEnabled;

            await _context.SaveChangesAsync();
        }
    }

    public class UserStats
    {
        public int TotalGames { get; set; }
        public int WonGames { get; set; }
        public double WinRate { get; set; }
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
        public double AverageGuesses { get; set; }
        public int TotalScore { get; set; }
        public List<Score> RecentScores { get; set; }
    }

    public class UserSettings
    {
        public string PreferredLanguage { get; set; }
        public string Theme { get; set; }
        public bool SoundEnabled { get; set; }
    }
} 