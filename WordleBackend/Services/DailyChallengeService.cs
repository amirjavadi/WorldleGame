using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;
using WordleBackend.Services.Interfaces;

namespace WordleBackend.Services;

public class DailyChallengeService : IDailyChallengeService
{
    private readonly AppDbContext _context;
    private readonly IWordService _wordService;
    private const int MaxAttempts = 6;

    public DailyChallengeService(AppDbContext context, IWordService wordService)
    {
        _context = context;
        _wordService = wordService;
    }

    public async Task<DailyChallenge> GetTodaysChallengeAsync()
    {
        var today = DateTime.UtcNow.Date;
        var challenge = await _context.DailyChallenges
            .Include(dc => dc.Word)
            .FirstOrDefaultAsync(dc => dc.Date == today);

        if (challenge == null)
        {
            challenge = await CreateChallengeAsync(today);
        }

        return challenge;
    }

    public async Task<DailyChallenge> CreateChallengeAsync(DateTime date)
    {
        var existingChallenge = await _context.DailyChallenges
            .Include(dc => dc.Word)
            .FirstOrDefaultAsync(dc => dc.Date == date.Date);

        if (existingChallenge != null)
            return existingChallenge;

        var word = await _wordService.GetRandomWordAsync();
        if (word == null)
        {
            word = new Word
            {
                Text = "سلام",
                Difficulty = "medium",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Words.Add(word);
            await _context.SaveChangesAsync();
        }

        var challenge = new DailyChallenge
        {
            WordId = word.Id,
            Word = word,
            Date = date.Date,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.DailyChallenges.Add(challenge);
        await _context.SaveChangesAsync();

        return challenge;
    }

    public async Task<DailyChallengeParticipation> StartParticipationAsync(int userId)
    {
        var challenge = await GetTodaysChallengeAsync();
        
        var existingParticipation = await _context.DailyChallengeParticipations
            .Include(p => p.Challenge)
                .ThenInclude(c => c.Word)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.ChallengeId == challenge.Id && p.UserId == userId);

        if (existingParticipation != null)
        {
            // محاسبه زمان باقی‌مانده تا چالش بعدی
            var nextChallengeTime = DateTime.UtcNow.Date.AddDays(1);
            var timeUntilNext = nextChallengeTime - DateTime.UtcNow;
            existingParticipation.TimeUntilNextChallenge = timeUntilNext.TotalMilliseconds;
            return existingParticipation;
        }

        var participation = new DailyChallengeParticipation
        {
            ChallengeId = challenge.Id,
            Challenge = challenge,
            UserId = userId,
            Score = 0,
            Attempts = 0,
            Status = "in_progress",
            StartTime = DateTime.UtcNow,
            Guesses = new List<string>(),
            TimeUntilNextChallenge = (DateTime.UtcNow.Date.AddDays(1) - DateTime.UtcNow).TotalMilliseconds
        };

        _context.DailyChallengeParticipations.Add(participation);
        await _context.SaveChangesAsync();

        // بارگذاری navigation properties
        await _context.Entry(participation)
            .Reference(p => p.Challenge)
            .Query()
            .Include(c => c.Word)
            .LoadAsync();

        await _context.Entry(participation)
            .Reference(p => p.User)
            .LoadAsync();

        return participation;
    }

    public async Task<DailyChallengeParticipation> MakeGuessAsync(int participationId, string guess)
    {
        var participation = await _context.DailyChallengeParticipations
            .Include(p => p.Challenge)
                .ThenInclude(c => c.Word)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == participationId);

        if (participation == null)
            throw new InvalidOperationException("مشارکت در چالش یافت نشد");

        // بررسی اینکه آیا این چالش برای امروز است
        if (participation.Challenge.Date.Date != DateTime.UtcNow.Date)
            throw new InvalidOperationException("این چالش مربوط به امروز نیست");

        if (participation.Status != "in_progress")
            throw new InvalidOperationException("این چالش به پایان رسیده است");

        if (participation.Attempts >= MaxAttempts)
            throw new InvalidOperationException("تعداد تلاش‌های مجاز به پایان رسیده است");

        if (string.IsNullOrWhiteSpace(guess))
            throw new InvalidOperationException("حدس نمی‌تواند خالی باشد");

        if (guess.Length != participation.Challenge.Word.Text.Length)
            throw new InvalidOperationException($"کلمه باید {participation.Challenge.Word.Text.Length} حرف باشد");

        participation.Attempts++;
        participation.Guesses ??= new List<string>();
        participation.Guesses.Add(guess);
        participation.LastGuess = guess;

        if (guess.Equals(participation.Challenge.Word.Text, StringComparison.OrdinalIgnoreCase))
        {
            participation.Status = "won";
            participation.EndTime = DateTime.UtcNow;
            participation.Score = CalculateScore(participation.Attempts, participation.Challenge.Word.Difficulty);
        }
        else if (participation.Attempts >= MaxAttempts)
        {
            participation.Status = "lost";
            participation.EndTime = DateTime.UtcNow;
            participation.Score = 0;
        }

        // محاسبه زمان باقی‌مانده تا چالش بعدی
        var nextChallengeTime = DateTime.UtcNow.Date.AddDays(1);
        var timeUntilNext = nextChallengeTime - DateTime.UtcNow;
        participation.TimeUntilNextChallenge = timeUntilNext.TotalMilliseconds;

        await _context.SaveChangesAsync();
        return participation;
    }

    public async Task<IEnumerable<DailyChallengeParticipation>> GetLeaderboardAsync(DateTime date, int limit = 10)
    {
        var challenge = await _context.DailyChallenges
            .FirstOrDefaultAsync(dc => dc.Date == date.Date);

        if (challenge == null)
            return Enumerable.Empty<DailyChallengeParticipation>();

        return await _context.DailyChallengeParticipations
            .Include(p => p.User)
            .Where(p => p.ChallengeId == challenge.Id && p.Status == "won")
            .OrderByDescending(p => p.Score)
            .ThenBy(p => p.Attempts)
            .ThenBy(p => p.EndTime)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<DailyChallengeParticipation?> GetUserParticipationAsync(int userId, DateTime date)
    {
        var challenge = await _context.DailyChallenges
            .Include(dc => dc.Word)
            .FirstOrDefaultAsync(dc => dc.Date == date.Date);

        if (challenge == null)
        {
            challenge = await CreateChallengeAsync(date);
        }

        var participation = await _context.DailyChallengeParticipations
            .Include(p => p.Challenge)
                .ThenInclude(c => c.Word)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.ChallengeId == challenge.Id && p.UserId == userId);

        if (participation != null)
        {
            // محاسبه زمان باقی‌مانده تا چالش بعدی
            var nextChallengeTime = date.Date.AddDays(1);
            var timeUntilNext = nextChallengeTime - DateTime.UtcNow;
            participation.TimeUntilNextChallenge = timeUntilNext.TotalMilliseconds;

            // اطمینان از بارگذاری داده‌های مورد نیاز
            if (participation.Challenge?.Word == null)
            {
                participation.Challenge = challenge;
            }
        }

        return participation;
    }

    public async Task<bool> HasUserParticipatedTodayAsync(int userId)
    {
        var today = DateTime.UtcNow.Date;
        var challenge = await _context.DailyChallenges
            .FirstOrDefaultAsync(dc => dc.Date == today);

        if (challenge == null)
            return false;

        return await _context.DailyChallengeParticipations
            .AnyAsync(p => p.ChallengeId == challenge.Id && p.UserId == userId);
    }

    public async Task<IEnumerable<DailyChallenge>> GetPastChallengesAsync(int days = 7)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.DailyChallenges
            .Include(dc => dc.Word)
            .Include(dc => dc.Participations)
            .Where(dc => dc.Date < today)
            .OrderByDescending(dc => dc.Date)
            .Take(days)
            .ToListAsync();
    }

    private int CalculateScore(int attempts, string difficulty)
    {
        var baseScore = 1000;
        var attemptPenalty = (attempts - 1) * 100;
        var difficultyMultiplier = difficulty.ToLower() switch
        {
            "easy" => 1.0,
            "medium" => 1.5,
            "hard" => 2.0,
            _ => 1.0
        };

        var finalScore = (int)((baseScore - attemptPenalty) * difficultyMultiplier);
        return Math.Max(0, finalScore);
    }
} 