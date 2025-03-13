using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Username { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        public virtual ICollection<GameHistory> GameHistories { get; set; }

        public int TotalGamesPlayed { get; set; }
        public int TotalGamesWon { get; set; }
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
    }
} 