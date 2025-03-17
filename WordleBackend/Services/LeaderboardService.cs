using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;
using WordleBackend.Services.Interfaces;

namespace WordleBackend.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly AppDbContext _context;

        public LeaderboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserLeaderboardEntry>> GetGlobalLeaderboardAsync(int page = 1, int pageSize = 10)
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
                    WinRate = (double)u.WonGames / u.TotalGames,
                    CurrentStreak = u.CurrentStreak,
                    MaxStreak = u.MaxStreak
                })
                .ToListAsync();
        }

        public async Task<List<UserLeaderboardEntry>> GetDailyLeaderboardAsync(DateTime date, int page = 1, int pageSize = 10)
        {
            return await _context.GameHistories
                .Include(gh => gh.User)
                .Where(gh => gh.StartTime.Date == date.Date && gh.Status == "Won")
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

        public async Task<UserLeaderboardEntry?> GetUserRankAsync(int userId)
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
            var totalWins = await _context.GameHistories.CountAsync(gh => gh.Status == "Won");
            var averageAttempts = await _context.GameHistories
                .Where(gh => gh.Status == "Won")
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

        public async Task UpdateLeaderboardAsync(GameHistory game)
        {
            var periods = new[] { "daily", "weekly", "monthly", "all-time" };
            foreach (var period in periods)
            {
                var (startDate, endDate) = GetPeriodDates(period);
                var stats = await GetOrCreateStats(game.UserId, period, startDate, endDate);

                // Update stats based on the game result
                stats.GamesPlayed++;
                if (game.Status == "won")
                {
                    stats.GamesWon++;
                    stats.TotalScore += game.Score;
                    stats.CurrentStreak++;
                    stats.BestStreak = Math.Max(stats.BestStreak, stats.CurrentStreak);
                }
                else if (game.Status == "lost")
                {
                    stats.CurrentStreak = 0;
                }

                stats.WinRate = (double)stats.GamesWon / stats.GamesPlayed;
                stats.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Leaderboard>> GetDailyLeaderboardAsync(int limit = 10)
        {
            var (startDate, endDate) = GetPeriodDates("daily");
            return await GetLeaderboardForPeriod("daily", startDate, endDate, limit);
        }

        public async Task<IEnumerable<Leaderboard>> GetWeeklyLeaderboardAsync(int limit = 10)
        {
            var (startDate, endDate) = GetPeriodDates("weekly");
            return await GetLeaderboardForPeriod("weekly", startDate, endDate, limit);
        }

        public async Task<IEnumerable<Leaderboard>> GetMonthlyLeaderboardAsync(int limit = 10)
        {
            var (startDate, endDate) = GetPeriodDates("monthly");
            return await GetLeaderboardForPeriod("monthly", startDate, endDate, limit);
        }

        public async Task<IEnumerable<Leaderboard>> GetAllTimeLeaderboardAsync(int limit = 10)
        {
            var (startDate, endDate) = GetPeriodDates("all-time");
            return await GetLeaderboardForPeriod("all-time", startDate, endDate, limit);
        }

        public async Task<Leaderboard> GetUserStatsAsync(int userId, string period = "all-time")
        {
            var (startDate, endDate) = GetPeriodDates(period);
            var stats = await _context.Leaderboards
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.UserId == userId && l.Period == period &&
                                        l.StartDate == startDate && l.EndDate == endDate);

            if (stats == null)
            {
                stats = await GetOrCreateStats(userId, period, startDate, endDate);
            }

            return stats;
        }

        private async Task<Leaderboard> GetOrCreateStats(int userId, string period, DateTime startDate, DateTime endDate)
        {
            var stats = await _context.Leaderboards
                .FirstOrDefaultAsync(l => l.UserId == userId && l.Period == period &&
                                        l.StartDate == startDate && l.EndDate == endDate);

            if (stats == null)
            {
                stats = new Leaderboard
                {
                    UserId = userId,
                    Period = period,
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalScore = 0,
                    GamesPlayed = 0,
                    GamesWon = 0,
                    WinRate = 0,
                    CurrentStreak = 0,
                    BestStreak = 0
                };
                _context.Leaderboards.Add(stats);
                await _context.SaveChangesAsync();
            }

            return stats;
        }

        private async Task<IEnumerable<Leaderboard>> GetLeaderboardForPeriod(string period, DateTime startDate, DateTime endDate, int limit)
        {
            return await _context.Leaderboards
                .Include(l => l.User)
                .Where(l => l.Period == period && l.StartDate == startDate && l.EndDate == endDate)
                .OrderByDescending(l => l.TotalScore)
                .ThenByDescending(l => l.WinRate)
                .ThenByDescending(l => l.BestStreak)
                .Take(limit)
                .ToListAsync();
        }

        private (DateTime startDate, DateTime endDate) GetPeriodDates(string period)
        {
            var now = DateTime.UtcNow;
            return period switch
            {
                "daily" => (now.Date, now.Date.AddDays(1).AddTicks(-1)),
                "weekly" => (now.AddDays(-(int)now.DayOfWeek).Date,
                            now.AddDays(7 - (int)now.DayOfWeek).Date.AddTicks(-1)),
                "monthly" => (new DateTime(now.Year, now.Month, 1),
                             new DateTime(now.Year, now.Month, 1).AddMonths(1).AddTicks(-1)),
                "all-time" => (new DateTime(2024, 1, 1), new DateTime(2099, 12, 31)),
                _ => throw new ArgumentException("Invalid period", nameof(period))
            };
        }
    }

    public class UserLeaderboardEntry
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int TotalGames { get; set; }
        public int WonGames { get; set; }
        public double WinRate { get; set; }
        public int CurrentStreak { get; set; }
        public int MaxStreak { get; set; }
        public int? Rank { get; set; }
        public int Score { get; set; }
        public int Attempts { get; set; }
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