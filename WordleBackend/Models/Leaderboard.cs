using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models;

public class Leaderboard
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int TotalScore { get; set; }

    [Required]
    public int GamesPlayed { get; set; }

    [Required]
    public int GamesWon { get; set; }

    [Required]
    public double WinRate { get; set; }

    [Required]
    public int CurrentStreak { get; set; }

    [Required]
    public int BestStreak { get; set; }

    [Required]
    public string Period { get; set; } = "daily"; // daily, weekly, monthly, all-time

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public User User { get; set; } = null!;
} 