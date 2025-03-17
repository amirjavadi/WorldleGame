using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordleBackend.Models;

namespace WordleBackend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(AppDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Add categories if none exist
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "حیوانات",
                        Description = "کلمات مرتبط با حیوانات",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    new Category
                    {
                        Name = "کشورها",
                        Description = "کلمات مرتبط با کشورها",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    new Category
                    {
                        Name = "غذاها",
                        Description = "کلمات مرتبط با غذاها",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Add words if none exist
            if (!await context.Words.AnyAsync())
            {
                var animals = await context.Categories.FirstAsync(c => c.Name == "حیوانات");
                var countries = await context.Categories.FirstAsync(c => c.Name == "کشورها");
                var food = await context.Categories.FirstAsync(c => c.Name == "غذاها");

                var words = new List<Word>
                {
                    // Animals - English
                    new Word { Text = "LION", CategoryId = animals.Id, Language = "en", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "ELEPHANT", CategoryId = animals.Id, Language = "en", Difficulty = "hard", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "TIGER", CategoryId = animals.Id, Language = "en", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "GIRAFFE", CategoryId = animals.Id, Language = "en", Difficulty = "hard", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "PENGUIN", CategoryId = animals.Id, Language = "en", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

                    // Animals - Persian
                    new Word { Text = "شیر", CategoryId = animals.Id, Language = "fa", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "فیل", CategoryId = animals.Id, Language = "fa", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "ببر", CategoryId = animals.Id, Language = "fa", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "زرافه", CategoryId = animals.Id, Language = "fa", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "پنگوئن", CategoryId = animals.Id, Language = "fa", Difficulty = "hard", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

                    // Countries - English
                    new Word { Text = "FRANCE", CategoryId = countries.Id, Language = "en", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "JAPAN", CategoryId = countries.Id, Language = "en", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "BRAZIL", CategoryId = countries.Id, Language = "en", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "AUSTRALIA", CategoryId = countries.Id, Language = "en", Difficulty = "hard", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "EGYPT", CategoryId = countries.Id, Language = "en", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

                    // Countries - Persian
                    new Word { Text = "فرانسه", CategoryId = countries.Id, Language = "fa", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "ژاپن", CategoryId = countries.Id, Language = "fa", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "برزیل", CategoryId = countries.Id, Language = "fa", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "استرالیا", CategoryId = countries.Id, Language = "fa", Difficulty = "hard", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "مصر", CategoryId = countries.Id, Language = "fa", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

                    // Food - English
                    new Word { Text = "PIZZA", CategoryId = food.Id, Language = "en", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "SUSHI", CategoryId = food.Id, Language = "en", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "PASTA", CategoryId = food.Id, Language = "en", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "CHOCOLATE", CategoryId = food.Id, Language = "en", Difficulty = "hard", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "BURGER", CategoryId = food.Id, Language = "en", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

                    // Food - Persian
                    new Word { Text = "پیتزا", CategoryId = food.Id, Language = "fa", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "سوشی", CategoryId = food.Id, Language = "fa", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "ماکارونی", CategoryId = food.Id, Language = "fa", Difficulty = "hard", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "شکلات", CategoryId = food.Id, Language = "fa", Difficulty = "medium", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Word { Text = "برگر", CategoryId = food.Id, Language = "fa", Difficulty = "easy", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };

                await context.Words.AddRangeAsync(words);
                await context.SaveChangesAsync();
            }

            // Add test users if none exist
            if (!await context.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Username = "admin",
                        Email = "admin@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsActive = true,
                        Role = "Admin"
                    },
                    new User
                    {
                        Username = "testuser1",
                        Email = "test1@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsActive = true
                    },
                    new User
                    {
                        Username = "testuser2",
                        Email = "test2@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsActive = true
                    }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }
    }
} 