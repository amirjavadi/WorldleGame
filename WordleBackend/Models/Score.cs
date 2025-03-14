using System;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class Score
    {
        public Score()
        {
            GameDifficulty = "medium";
            User = null!;
            Points = 0;
            Attempts = 0;
            GuessTime = 0;
            IsWin = false;
            SpeedBonus = 0;
            DifficultyBonus = 0;
            StreakBonus = 0;
            CreatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public string GameDifficulty { get; set; }
        public int Attempts { get; set; }
        public double GuessTime { get; set; }
        public bool IsWin { get; set; }
        public DateTime CreatedAt { get; set; }

        // Bonus points
        public int SpeedBonus { get; set; }
        public int DifficultyBonus { get; set; }
        public int StreakBonus { get; set; }

        // Total score calculation
        public int Value => Points + SpeedBonus + DifficultyBonus + StreakBonus;

        // Navigation property
        public virtual User User { get; set; }
    }
} 