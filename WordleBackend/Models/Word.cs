using System;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class Word
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Value { get; set; }

        public DateTime? UsedOn { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } // Admin username who added the word
    }
} 