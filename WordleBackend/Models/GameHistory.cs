using System;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class GameHistory
    {
        public GameHistory()
        {
            UserId = string.Empty;
            Guesses = string.Empty;
            User = null!;
            Word = null!;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public int WordId { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
        public int Attempts { get; set; }
        public bool IsWon { get; set; }
        public string Guesses { get; set; } // JSON string of all guesses
        public int Score { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Word Word { get; set; }
    }
} 