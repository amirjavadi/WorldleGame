using System;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class GameHistory
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public string Word { get; set; }

        public int Attempts { get; set; }

        public bool IsWon { get; set; }

        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;

        public string Guesses { get; set; } // JSON string of all guesses made

        public int Score { get; set; }

        public string Feedback { get; set; } // JSON string of feedback for each guess
    }
} 