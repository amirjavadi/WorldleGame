using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class Word
    {
        public Word()
        {
            Text = string.Empty;
            CreatedBy = string.Empty;
            Difficulty = "Normal";
            Language = "en";
            GameHistories = new List<GameHistory>();
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Text { get; set; }

        [Required]
        [MaxLength(20)]
        public string Difficulty { get; set; }

        [Required]
        [MaxLength(10)]
        public string Language { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        // Foreign key for Category
        public int CategoryId { get; set; }

        // Navigation property
        public virtual Category Category { get; set; }

        // Navigation property
        public virtual ICollection<GameHistory> GameHistories { get; set; }
    }
} 