using WordleBackend.Models;

namespace WordleBackend.Services.Interfaces;

public interface IStatisticsService
{
    Task<UserStatistics> GetUserStatisticsAsync(int userId);
    Task<GlobalStatistics> GetGlobalStatisticsAsync();
    Task<CategoryStatistics> GetCategoryStatisticsAsync(int categoryId);
    Task<IEnumerable<DailyStatistics>> GetDailyStatisticsAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<WordStatistics>> GetMostPlayedWordsAsync(int limit = 10);
    Task<IEnumerable<WordStatistics>> GetHardestWordsAsync(int limit = 10);
    Task<IEnumerable<WordStatistics>> GetEasiestWordsAsync(int limit = 10);
}

public class UserStatistics
{
    public int TotalGames { get; set; }
    public int WonGames { get; set; }
    public int LostGames { get; set; }
    public double WinRate { get; set; }
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
    public double AverageAttempts { get; set; }
    public Dictionary<int, int> AttemptsDistribution { get; set; } = new();
    public Dictionary<string, int> CategoryDistribution { get; set; } = new();
    public Dictionary<string, int> LanguageDistribution { get; set; } = new();
}

public class GlobalStatistics
{
    public int TotalGames { get; set; }
    public int TotalPlayers { get; set; }
    public int ActivePlayers { get; set; }
    public double AverageWinRate { get; set; }
    public double AverageAttempts { get; set; }
    public Dictionary<string, int> CategoryDistribution { get; set; } = new();
    public Dictionary<string, int> LanguageDistribution { get; set; } = new();
    public Dictionary<string, int> DifficultyDistribution { get; set; } = new();
}

public class CategoryStatistics
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int TotalWords { get; set; }
    public int TotalGames { get; set; }
    public double AverageWinRate { get; set; }
    public double AverageAttempts { get; set; }
    public Dictionary<string, int> DifficultyDistribution { get; set; } = new();
    public Dictionary<string, int> LanguageDistribution { get; set; } = new();
}

public class DailyStatistics
{
    public DateTime Date { get; set; }
    public int TotalGames { get; set; }
    public int UniquePlayers { get; set; }
    public double WinRate { get; set; }
    public double AverageAttempts { get; set; }
}

public class WordStatistics
{
    public int WordId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public int TimesPlayed { get; set; }
    public int TimesWon { get; set; }
    public double WinRate { get; set; }
    public double AverageAttempts { get; set; }
} 