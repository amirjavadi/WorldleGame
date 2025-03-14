using System;

namespace WordleBackend.Models
{
    public class SystemSettings
    {
        public SystemSettings()
        {
            DefaultDifficulty = "medium";
        }

        public int Id { get; set; }
        public int MaxDailyGames { get; set; }
        public string DefaultDifficulty { get; set; }
        public bool EnableGuestPlay { get; set; }
        public bool MaintenanceMode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 