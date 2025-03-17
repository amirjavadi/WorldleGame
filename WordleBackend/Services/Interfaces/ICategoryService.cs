using WordleBackend.Models;

namespace WordleBackend.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
    Task<IEnumerable<Word>> GetWordsByCategoryAsync(int categoryId);
    Task<Category?> GetCategoryByNameAsync(string name);
    Task<bool> AssignWordToCategoryAsync(int wordId, int categoryId);
    Task<bool> RemoveWordFromCategoryAsync(int wordId, int categoryId);
} 