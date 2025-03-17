using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WordleBackend.Models
{
    public class GameHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int WordId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required]
        public string Status { get; set; } = "in_progress"; // in_progress, won, lost

        [Required]
        public int Attempts { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public string Difficulty { get; set; } = "medium"; // easy, medium, hard

        [Required]
        public List<string> Guesses { get; set; } = new();

        public string? LastGuess { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Word Word { get; set; } = null!;
    }
} 