using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class User
    {
        public User()
        {
            Username = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
            DisplayName = string.Empty;
            Avatar = string.Empty;
            Bio = string.Empty;
            Role = "User";
            PreferredLanguage = "fa";
            Theme = "light";
            GameHistories = new List<GameHistory>();
            Scores = new List<Score>();
            TotalGames = 0;
            WonGames = 0;
            CurrentStreak = 0;
            MaxStreak = 0;
            TotalScore = 0;
            SoundEnabled = true;
            IsAdmin = false;
            JoinDate = DateTime.UtcNow;
            LastLoginAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        public string Role { get; set; }
        public bool IsAdmin { get; set; }

        // Profile Info
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }

        // Stats
        public int TotalGames { get; set; }
        public int WonGames { get; set; }
        public int CurrentStreak { get; set; }
        public int MaxStreak { get; set; }
        public int TotalScore { get; set; }

        // Settings
        public string PreferredLanguage { get; set; }
        public string Theme { get; set; }
        public bool SoundEnabled { get; set; }

        // Timestamps
        public DateTime JoinDate { get; set; }
        public DateTime LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<GameHistory> GameHistories { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
} 