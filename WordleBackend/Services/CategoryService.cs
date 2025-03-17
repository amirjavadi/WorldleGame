using Microsoft.EntityFrameworkCore;
using WordleBackend.Data;
using WordleBackend.Models;
using WordleBackend.Services.Interfaces;

namespace WordleBackend.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Include(c => c.Words)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.Words)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
            throw new InvalidOperationException($"دسته‌بندی با نام {category.Name} قبلاً وجود دارد");

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        var existingCategory = await _context.Categories.FindAsync(category.Id);
        if (existingCategory == null)
            throw new InvalidOperationException("دسته‌بندی مورد نظر یافت نشد");

        if (await _context.Categories.AnyAsync(c => c.Name == category.Name && c.Id != category.Id))
            throw new InvalidOperationException($"دسته‌بندی با نام {category.Name} قبلاً وجود دارد");

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        existingCategory.DescriptionPersian = category.DescriptionPersian;
        existingCategory.IsActive = category.IsActive;
        existingCategory.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingCategory;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            throw new InvalidOperationException("دسته‌بندی مورد نظر یافت نشد");

        var hasWords = await _context.Words.AnyAsync(w => w.CategoryId == id);
        if (hasWords)
            throw new InvalidOperationException("این دسته‌بندی دارای کلمات است و نمی‌تواند حذف شود");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Word>> GetWordsByCategoryAsync(int categoryId)
    {
        return await _context.Words
            .Where(w => w.CategoryId == categoryId)
            .OrderBy(w => w.Text)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        return await _context.Categories
            .Include(c => c.Words)
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<bool> AssignWordToCategoryAsync(int wordId, int categoryId)
    {
        var word = await _context.Words.FindAsync(wordId);
        if (word == null)
            throw new InvalidOperationException("کلمه مورد نظر یافت نشد");

        var category = await _context.Categories.FindAsync(categoryId);
        if (category == null)
            throw new InvalidOperationException("دسته‌بندی مورد نظر یافت نشد");

        word.CategoryId = categoryId;
        word.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveWordFromCategoryAsync(int wordId, int categoryId)
    {
        var word = await _context.Words.FindAsync(wordId);
        if (word == null || word.CategoryId != categoryId)
            return false;

        word.CategoryId = 0;
        word.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
} 