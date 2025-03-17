using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WordleBackend.Models;

public class DailyChallenge
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int WordId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public Word Word { get; set; } = null!;
    
    [JsonIgnore]
    public ICollection<DailyChallengeParticipation> Participations { get; set; } = new List<DailyChallengeParticipation>();
}

public class DailyChallengeParticipation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ChallengeId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int Score { get; set; }

    [Required]
    public int Attempts { get; set; }

    [Required]
    public string Status { get; set; } = "in_progress"; // in_progress, won, lost

    [Required]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    public DateTime? EndTime { get; set; }

    public List<string> Guesses { get; set; } = new();

    public string? LastGuess { get; set; }

    public double TimeUntilNextChallenge { get; set; }

    // Navigation properties
    [JsonIgnore]
    public DailyChallenge Challenge { get; set; } = null!;
    
    [JsonIgnore]
    public User User { get; set; } = null!;

    // DTO properties
    public string? WordText => Challenge?.Word?.Text;
    public string? Username => User?.Username;
} 