using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;

namespace WordleBackend.Services
{
    public interface ILeaderboardService
    {
        Task<IEnumerable<UserLeaderboardEntry>> GetGlobalLeaderboardAsync(int page = 1, int pageSize = 10);
        Task<IEnumerable<UserLeaderboardEntry>> GetDailyLeaderboardAsync(DateTime date, int page = 1, int pageSize = 10);
        Task<UserLeaderboardEntry?> GetUserRankAsync(string userId);
        Task<GameStatistics> GetGameStatisticsAsync();
    }

    public class LeaderboardService : ILeaderboardService
    {
        private readonly AppDbContext _context;

        public LeaderboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserLeaderboardEntry>> GetGlobalLeaderboardAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Users
                .Where(u => u.TotalGames > 0)
                .OrderByDescending(u => u.WonGames)
                .ThenByDescending(u => u.MaxStreak)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserLeaderboardEntry
                {
                    UserId = u.Id,
                    Username = u.Username,
                    TotalGames = u.TotalGames,
                    WonGames = u.WonGames,
                    WinRate = u.TotalGames > 0 ? (double)u.WonGames / u.TotalGames : 0,
                    CurrentStreak = u.CurrentStreak,
                    MaxStreak = u.MaxStreak
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<UserLeaderboardEntry>> GetDailyLeaderboardAsync(DateTime date, int page = 1, int pageSize = 10)
        {
            return await _context.GameHistories
                .Include(gh => gh.User)
                .Where(gh => gh.PlayedAt.Date == date.Date && gh.IsWon)
                .OrderBy(gh => gh.Attempts)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(gh => new UserLeaderboardEntry
                {
                    UserId = gh.UserId,
                    Username = gh.User.Username,
                    Score = gh.Score,
                    Attempts = gh.Attempts
                })
                .ToListAsync();
        }

        public async Task<UserLeaderboardEntry?> GetUserRankAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.TotalGames == 0)
            {
                return null;
            }

            var rank = await _context.Users
                .CountAsync(u => u.TotalGames > 0 && 
                    (u.WonGames > user.WonGames || 
                    (u.WonGames == user.WonGames && u.MaxStreak > user.MaxStreak)));

            return new UserLeaderboardEntry
            {
                UserId = user.Id,
                Username = user.Username,
                TotalGames = user.TotalGames,
                WonGames = user.WonGames,
                WinRate = (double)user.WonGames / user.TotalGames,
                CurrentStreak = user.CurrentStreak,
                MaxStreak = user.MaxStreak,
                Rank = rank + 1
            };
        }

        public async Task<GameStatistics> GetGameStatisticsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var activeUsers = await _context.Users.CountAsync(u => u.TotalGames > 0);
            var totalGames = await _context.GameHistories.CountAsync();
            var totalWins = await _context.GameHistories.CountAsync(gh => gh.IsWon);
            var averageAttempts = await _context.GameHistories
                .Where(gh => gh.IsWon)
                .AverageAsync(gh => gh.Attempts);

            return new GameStatistics
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                TotalGames = totalGames,
                TotalWins = totalWins,
                WinRate = totalGames > 0 ? (double)totalWins / totalGames : 0,
                AverageAttempts = averageAttempts
            };
        }
    }

    public class UserLeaderboardEntry
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int TotalGames { get; set; }
        public int WonGames { get; set; }
        public double WinRate { get; set; }
        public int CurrentStreak { get; set; }
        public int MaxStreak { get; set; }
        public int Score { get; set; }
        public int Attempts { get; set; }
        public int? Rank { get; set; }
    }

    public class GameStatistics
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalGames { get; set; }
        public int TotalWins { get; set; }
        public double WinRate { get; set; }
        public double AverageAttempts { get; set; }
    }
} 