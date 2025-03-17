using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;

namespace WordleBackend.Services
{
    public interface IWordService
    {
        Task<Word> AddWordAsync(string text, int categoryId);
        Task<Word> SetTodaysWordAsync(string text);
        Task<bool> IsValidWordAsync(string text);
        Task<IEnumerable<Word>> GetWordsAsync(int page = 1, int pageSize = 10);
        Task<Word?> GetWordByIdAsync(int id);
        Task<bool> DeleteWordAsync(int id);
        Task<Word?> GetRandomWordAsync();
    }

    public class WordService : IWordService
    {
        private readonly AppDbContext _context;

        public WordService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Word> AddWordAsync(string text, int categoryId)
        {
            // بررسی وجود دسته‌بندی
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"دسته‌بندی با شناسه {categoryId} یافت نشد");
            }

            // بررسی تکراری نبودن کلمه
            if (await _context.Words.AnyAsync(w => w.Text == text))
            {
                throw new Exception("این کلمه قبلاً در دیتابیس وجود دارد");
            }

            var word = new Word
            {
                Text = text,
                CategoryId = categoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                Language = "fa",
                Difficulty = "medium",
                CreatedBy = "System"
            };

            _context.Words.Add(word);
            await _context.SaveChangesAsync();
            return word;
        }

        public async Task<Word> SetTodaysWordAsync(string text)
        {
            var today = DateTime.UtcNow.Date;

            // غیرفعال کردن کلمه روز قبلی
            var previousWord = await _context.Words
                .FirstOrDefaultAsync(w => w.CreatedAt.Date == today && w.IsActive);

            if (previousWord != null)
            {
                previousWord.IsActive = false;
            }

            // پیدا کردن یا ایجاد کلمه جدید
            var word = await _context.Words
                .FirstOrDefaultAsync(w => w.Text == text.ToUpper());

            if (word == null)
            {
                word = new Word
                {
                    Text = text.ToUpper(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    Language = "en",
                    Difficulty = "medium"
                };
                _context.Words.Add(word);
            }
            else
            {
                word.IsActive = true;
                word.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return word;
        }

        public async Task<bool> IsValidWordAsync(string text)
        {
            return await _context.Words.AnyAsync(w => w.Text == text.ToUpper());
        }

        public async Task<IEnumerable<Word>> GetWordsAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Words
                .Include(w => w.Category)
                .OrderByDescending(w => w.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Word?> GetWordByIdAsync(int id)
        {
            return await _context.Words
                .Include(w => w.Category)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<bool> DeleteWordAsync(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word == null)
            {
                return false;
            }

            _context.Words.Remove(word);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Word?> GetRandomWordAsync()
        {
            var count = await _context.Words.CountAsync(w => w.IsActive);
            if (count == 0)
                return null;

            var random = new Random();
            var skip = random.Next(0, count);

            return await _context.Words
                .Where(w => w.IsActive)
                .Skip(skip)
                .FirstOrDefaultAsync();
        }
    }
} 