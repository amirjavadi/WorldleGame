using System;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class GameHistory
    {
        public GameHistory()
        {
            Guesses = string.Empty;
            LastGuess = string.Empty;
            Status = "InProgress";
            StartTime = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int WordId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Attempts { get; set; }
        public string Status { get; set; } // "InProgress", "Won", "Lost"
        public string LastGuess { get; set; }
        public string Guesses { get; set; } // JSON string of all guesses
        public int Score { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Word Word { get; set; }
    }
} 