using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;
using WordleBackend.Services.Interfaces;

namespace WordleBackend.Services;

public class StatisticsService : IStatisticsService
{
    private readonly AppDbContext _context;

    public StatisticsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserStatistics> GetUserStatisticsAsync(int userId)
    {
        var games = await _context.GameHistories
            .Include(gh => gh.Word)
            .ThenInclude(w => w.Category)
            .Where(gh => gh.UserId == userId)
            .ToListAsync();

        if (!games.Any())
            return new UserStatistics();

        var wonGames = games.Count(g => g.Status == "won");
        var stats = new UserStatistics
        {
            TotalGames = games.Count,
            WonGames = wonGames,
            LostGames = games.Count(g => g.Status == "lost"),
            WinRate = (double)wonGames / games.Count,
            CurrentStreak = await CalculateCurrentStreakAsync(userId),
            BestStreak = await CalculateBestStreakAsync(userId),
            AverageAttempts = games.Where(g => g.Status == "won").Average(g => g.Attempts),
            AttemptsDistribution = games.Where(g => g.Status == "won")
                .GroupBy(g => g.Attempts)
                .ToDictionary(g => g.Key, g => g.Count()),
            CategoryDistribution = games
                .GroupBy(g => g.Word.Category.Name)
                .ToDictionary(g => g.Key, g => g.Count()),
            LanguageDistribution = games
                .GroupBy(g => g.Word.Language)
                .ToDictionary(g => g.Key, g => g.Count())
        };

        return stats;
    }

    public async Task<GlobalStatistics> GetGlobalStatisticsAsync()
    {
        var games = await _context.GameHistories
            .Include(gh => gh.Word)
            .ThenInclude(w => w.Category)
            .ToListAsync();

        var totalPlayers = await _context.Users.CountAsync();
        var activePlayers = await _context.Users.CountAsync(u => u.TotalGames > 0);
        var wonGames = games.Count(g => g.Status == "won");

        return new GlobalStatistics
        {
            TotalGames = games.Count,
            TotalPlayers = totalPlayers,
            ActivePlayers = activePlayers,
            AverageWinRate = games.Any() ? (double)wonGames / games.Count : 0,
            AverageAttempts = games.Where(g => g.Status == "won").Average(g => g.Attempts),
            CategoryDistribution = games
                .GroupBy(g => g.Word.Category.Name)
                .ToDictionary(g => g.Key, g => g.Count()),
            LanguageDistribution = games
                .GroupBy(g => g.Word.Language)
                .ToDictionary(g => g.Key, g => g.Count()),
            DifficultyDistribution = games
                .GroupBy(g => g.Word.Difficulty)
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }

    public async Task<CategoryStatistics> GetCategoryStatisticsAsync(int categoryId)
    {
        var category = await _context.Categories
            .Include(c => c.Words)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category == null)
            throw new InvalidOperationException("دسته‌بندی مورد نظر یافت نشد");

        var games = await _context.GameHistories
            .Include(gh => gh.Word)
            .Where(gh => gh.Word.CategoryId == categoryId)
            .ToListAsync();

        var wonGames = games.Count(g => g.Status == "won");

        return new CategoryStatistics
        {
            CategoryId = category.Id,
            CategoryName = category.Name,
            TotalWords = category.Words.Count,
            TotalGames = games.Count,
            AverageWinRate = games.Any() ? (double)wonGames / games.Count : 0,
            AverageAttempts = games.Where(g => g.Status == "won").Average(g => g.Attempts),
            DifficultyDistribution = category.Words
                .GroupBy(w => w.Difficulty)
                .ToDictionary(g => g.Key, g => g.Count()),
            LanguageDistribution = category.Words
                .GroupBy(w => w.Language)
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }

    public async Task<IEnumerable<DailyStatistics>> GetDailyStatisticsAsync(DateTime startDate, DateTime endDate)
    {
        var games = await _context.GameHistories
            .Where(gh => gh.StartTime.Date >= startDate.Date && gh.StartTime.Date <= endDate.Date)
            .ToListAsync();

        return games.GroupBy(g => g.StartTime.Date)
            .Select(g => new DailyStatistics
            {
                Date = g.Key,
                TotalGames = g.Count(),
                UniquePlayers = g.Select(gh => gh.UserId).Distinct().Count(),
                WinRate = (double)g.Count(gh => gh.Status == "won") / g.Count(),
                AverageAttempts = g.Where(gh => gh.Status == "won").Average(gh => gh.Attempts)
            })
            .OrderByDescending(s => s.Date);
    }

    public async Task<IEnumerable<WordStatistics>> GetMostPlayedWordsAsync(int limit = 10)
    {
        return await GetWordStatisticsAsync(words => words
            .OrderByDescending(w => w.TimesPlayed)
            .Take(limit));
    }

    public async Task<IEnumerable<WordStatistics>> GetHardestWordsAsync(int limit = 10)
    {
        return await GetWordStatisticsAsync(words => words
            .Where(w => w.TimesPlayed >= 5) // Minimum games threshold
            .OrderBy(w => w.WinRate)
            .ThenByDescending(w => w.AverageAttempts)
            .Take(limit));
    }

    public async Task<IEnumerable<WordStatistics>> GetEasiestWordsAsync(int limit = 10)
    {
        return await GetWordStatisticsAsync(words => words
            .Where(w => w.TimesPlayed >= 5) // Minimum games threshold
            .OrderByDescending(w => w.WinRate)
            .ThenBy(w => w.AverageAttempts)
            .Take(limit));
    }

    private async Task<IEnumerable<WordStatistics>> GetWordStatisticsAsync(
        Func<IEnumerable<WordStatistics>, IEnumerable<WordStatistics>> orderAndFilter)
    {
        var words = await _context.Words
            .Include(w => w.Category)
            .Include(w => w.GameHistories)
            .ToListAsync();

        var wordStats = words.Select(w =>
        {
            var games = w.GameHistories;
            var wonGames = games.Count(g => g.Status == "won");
            return new WordStatistics
            {
                WordId = w.Id,
                Text = w.Text,
                Category = w.Category?.Name ?? "",
                Language = w.Language,
                Difficulty = w.Difficulty,
                TimesPlayed = games.Count,
                TimesWon = wonGames,
                WinRate = games.Any() ? (double)wonGames / games.Count : 0,
                AverageAttempts = games.Where(g => g.Status == "won").Average(g => g.Attempts)
            };
        });

        return orderAndFilter(wordStats);
    }

    private async Task<int> CalculateCurrentStreakAsync(int userId)
    {
        var games = await _context.GameHistories
            .Where(gh => gh.UserId == userId)
            .OrderByDescending(gh => gh.StartTime)
            .ToListAsync();

        var streak = 0;
        foreach (var game in games)
        {
            if (game.Status == "won")
                streak++;
            else
                break;
        }
        return streak;
    }

    private async Task<int> CalculateBestStreakAsync(int userId)
    {
        var games = await _context.GameHistories
            .Where(gh => gh.UserId == userId)
            .OrderBy(gh => gh.StartTime)
            .ToListAsync();

        var currentStreak = 0;
        var bestStreak = 0;

        foreach (var game in games)
        {
            if (game.Status == "won")
            {
                currentStreak++;
                bestStreak = Math.Max(bestStreak, currentStreak);
            }
            else
            {
                currentStreak = 0;
            }
        }

        return bestStreak;
    }
} 