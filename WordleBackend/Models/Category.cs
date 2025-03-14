using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordleBackend.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        // Navigation property
        public virtual ICollection<Word> Words { get; set; }

        public Category()
        {
            Words = new HashSet<Word>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
} 