using System;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class Word
    {
        public int Id { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public string CreatedBy { get; set; } // Admin username who added the word
    }
} 