using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;

namespace WordleBackend.Services
{
    public interface IWordService
    {
        Task<Word> AddWordAsync(string text);
        Task<Word> SetTodaysWordAsync(string text);
        Task<bool> IsValidWordAsync(string text);
        Task<IEnumerable<Word>> GetWordsAsync(int page = 1, int pageSize = 10);
        Task<Word?> GetWordByIdAsync(int id);
        Task<bool> DeleteWordAsync(int id);
    }

    public class WordService : IWordService
    {
        private readonly AppDbContext _context;

        public WordService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Word> AddWordAsync(string text)
        {
            if (await _context.Words.AnyAsync(w => w.Text == text))
            {
                throw new Exception("این کلمه قبلاً در دیتابیس وجود دارد");
            }

            var word = new Word
            {
                Text = text.ToUpper(),
                CreatedAt = DateTime.UtcNow,
                IsActive = false
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
                .FirstOrDefaultAsync(w => w.Date.Date == today && w.IsActive);

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
                    IsActive = true,
                    Date = today
                };
                _context.Words.Add(word);
            }
            else
            {
                word.IsActive = true;
                word.Date = today;
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
                .OrderByDescending(w => w.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Word?> GetWordByIdAsync(int id)
        {
            return await _context.Words.FindAsync(id);
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
    }
} 