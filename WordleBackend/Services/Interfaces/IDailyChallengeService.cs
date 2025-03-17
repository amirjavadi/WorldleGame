using WordleBackend.Models;

namespace WordleBackend.Services.Interfaces;

public interface IDailyChallengeService
{
    Task<DailyChallenge> GetTodaysChallengeAsync();
    Task<DailyChallenge> CreateChallengeAsync(DateTime date);
    Task<DailyChallengeParticipation> StartParticipationAsync(int userId);
    Task<DailyChallengeParticipation> MakeGuessAsync(int participationId, string guess);
    Task<IEnumerable<DailyChallengeParticipation>> GetLeaderboardAsync(DateTime date, int limit = 10);
    Task<DailyChallengeParticipation?> GetUserParticipationAsync(int userId, DateTime date);
    Task<bool> HasUserParticipatedTodayAsync(int userId);
    Task<IEnumerable<DailyChallenge>> GetPastChallengesAsync(int days = 7);
} 