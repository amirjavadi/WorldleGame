using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            GameHistories = new List<GameHistory>();
        }

        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        public virtual ICollection<GameHistory> GameHistories { get; set; }

        public int TotalGames { get; set; }
        public int WonGames { get; set; }
        public int CurrentStreak { get; set; }
        public int MaxStreak { get; set; }
    }
} 