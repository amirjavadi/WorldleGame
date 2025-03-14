using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;

namespace WordleBackend.Services
{
    public interface IAdminService
    {
        // مدیریت کاربران
        Task<List<User>> GetAllUsersAsync(int page = 1, int pageSize = 10);
        Task<User> UpdateUserRoleAsync(int userId, string role);
        Task<bool> DeleteUserAsync(int userId);
        Task<User> GetUserDetailsAsync(int userId);

        // مدیریت کلمات
        Task<IEnumerable<Word>> GetAllWordsAsync(int page = 1, int pageSize = 10);
        Task<Word> AddWordAsync(Word word);
        Task<Word> UpdateWordAsync(int wordId, Word word);
        Task<bool> DeleteWordAsync(int wordId);
        Task<Word> GetWordDetailsAsync(int wordId);

        // گزارشات و آمار
        Task<AdminDashboardStats> GetDashboardStatsAsync();
        Task<List<GameHistory>> GetRecentGamesAsync(int count = 10);
        Task<SystemSettings> GetSystemSettingsAsync();
        Task UpdateSystemSettingsAsync(SystemSettings settings);
    }

    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        // مدیریت کاربران
        public async Task<List<User>> GetAllUsersAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Users
                .OrderByDescending(u => u.JoinDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<User> UpdateUserRoleAsync(int userId, string role)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("کاربر یافت نشد");

            user.Role = role;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserDetailsAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.GameHistories)
                .Include(u => u.Scores)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("کاربر یافت نشد");

            return user;
        }

        // مدیریت کلمات
        public async Task<IEnumerable<Word>> GetAllWordsAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Words
                .Include(w => w.Category)
                .OrderByDescending(w => w.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Word> AddWordAsync(Word word)
        {
            word.CreatedAt = DateTime.UtcNow;
            word.UpdatedAt = DateTime.UtcNow;
            _context.Words.Add(word);
            await _context.SaveChangesAsync();
            return word;
        }

        public async Task<Word> UpdateWordAsync(int wordId, Word word)
        {
            var existingWord = await _context.Words.FindAsync(wordId);
            if (existingWord == null)
                throw new KeyNotFoundException("کلمه یافت نشد");

            existingWord.Text = word.Text;
            existingWord.Difficulty = word.Difficulty;
            existingWord.Language = word.Language;
            existingWord.CategoryId = word.CategoryId;
            existingWord.IsActive = word.IsActive;
            existingWord.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingWord;
        }

        public async Task<bool> DeleteWordAsync(int wordId)
        {
            var word = await _context.Words.FindAsync(wordId);
            if (word == null)
                return false;

            _context.Words.Remove(word);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Word> GetWordDetailsAsync(int wordId)
        {
            var word = await _context.Words
                .Include(w => w.Category)
                .Include(w => w.GameHistories)
                .FirstOrDefaultAsync(w => w.Id == wordId);

            if (word == null)
                throw new KeyNotFoundException("کلمه یافت نشد");

            return word;
        }

        // گزارشات و آمار
        public async Task<AdminDashboardStats> GetDashboardStatsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalGames = await _context.GameHistories.CountAsync();
            var activeUsers = await _context.Users
                .Where(u => u.LastLoginAt >= DateTime.UtcNow.AddDays(-7))
                .CountAsync();

            var topPlayers = await _context.Users
                .OrderByDescending(u => u.TotalScore)
                .Take(5)
                .Select(u => new { u.Username, u.TotalScore })
                .ToListAsync();

            return new AdminDashboardStats
            {
                TotalUsers = totalUsers,
                TotalGames = totalGames,
                ActiveUsers = activeUsers,
                TopPlayers = topPlayers.Select(p => new TopPlayer 
                { 
                    Username = p.Username, 
                    Score = p.TotalScore 
                }).ToList()
            };
        }

        public async Task<List<GameHistory>> GetRecentGamesAsync(int count = 10)
        {
            return await _context.GameHistories
                .Include(g => g.User)
                .OrderByDescending(g => g.StartTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<SystemSettings> GetSystemSettingsAsync()
        {
            var settings = await _context.SystemSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new SystemSettings
                {
                    MaxDailyGames = 10,
                    DefaultDifficulty = "medium",
                    EnableGuestPlay = true,
                    MaintenanceMode = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.SystemSettings.Add(settings);
                await _context.SaveChangesAsync();
            }
            return settings;
        }

        public async Task UpdateSystemSettingsAsync(SystemSettings settings)
        {
            var existingSettings = await _context.SystemSettings.FirstOrDefaultAsync();
            if (existingSettings == null)
            {
                settings.CreatedAt = DateTime.UtcNow;
                settings.UpdatedAt = DateTime.UtcNow;
                _context.SystemSettings.Add(settings);
            }
            else
            {
                existingSettings.MaxDailyGames = settings.MaxDailyGames;
                existingSettings.DefaultDifficulty = settings.DefaultDifficulty;
                existingSettings.EnableGuestPlay = settings.EnableGuestPlay;
                existingSettings.MaintenanceMode = settings.MaintenanceMode;
                existingSettings.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }

    public class AdminDashboardStats
    {
        public int TotalUsers { get; set; }
        public int TotalGames { get; set; }
        public int ActiveUsers { get; set; }
        public List<TopPlayer> TopPlayers { get; set; }
    }

    public class TopPlayer
    {
        public string Username { get; set; }
        public int Score { get; set; }
    }
} 