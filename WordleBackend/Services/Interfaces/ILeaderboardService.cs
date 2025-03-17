using WordleBackend.Models;

namespace WordleBackend.Services.Interfaces;

public interface ILeaderboardService
{
    Task UpdateLeaderboardAsync(GameHistory game);
    Task<IEnumerable<Leaderboard>> GetDailyLeaderboardAsync(int limit = 10);
    Task<IEnumerable<Leaderboard>> GetWeeklyLeaderboardAsync(int limit = 10);
    Task<IEnumerable<Leaderboard>> GetMonthlyLeaderboardAsync(int limit = 10);
    Task<IEnumerable<Leaderboard>> GetAllTimeLeaderboardAsync(int limit = 10);
    Task<Leaderboard> GetUserStatsAsync(int userId, string period = "all-time");
} 