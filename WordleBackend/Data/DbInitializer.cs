using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WordleBackend.Models;

namespace WordleBackend.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // اطمینان از ایجاد دیتابیس
            context.Database.EnsureCreated();

            // بررسی وجود داده
            if (context.Users.Any())
            {
                return; // اگر داده وجود دارد، از اضافه کردن داده‌های اولیه صرف نظر می‌کنیم
            }

            // اضافه کردن کاربر ادمین
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@wordle.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // در محیط تولید از رمز قوی‌تر استفاده کنید
                Role = "Admin",
                IsAdmin = true,
                DisplayName = "مدیر سیستم",
                Avatar = "default-avatar.png",
                Bio = "مدیر سیستم Wordle",
                PreferredLanguage = "fa",
                Theme = "light",
                SoundEnabled = true,
                JoinDate = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Users.Add(adminUser);

            // اضافه کردن دسته‌بندی‌ها
            if (!context.Categories.Any())
            {
                var categories = new Category[]
                {
                    new Category
                    {
                        Name = "Animals",
                        Description = "Words related to animals",
                        CreatedBy = "System",
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Food",
                        Description = "Words related to food and drinks",
                        CreatedBy = "System",
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Countries",
                        Description = "Names of countries",
                        CreatedBy = "System",
                        IsActive = true
                    }
                };

                foreach (Category category in categories)
                {
                    context.Categories.Add(category);
                }
                context.SaveChanges();
            }

            // اضافه کردن کلمات پیش‌فرض
            if (!context.Words.Any())
            {
                var words = new Word[]
                {
                    new Word
                    {
                        Text = "LION",
                        Difficulty = "Easy",
                        Language = "en",
                        CategoryId = 1, // Animals
                        CreatedBy = "System",
                        IsActive = true
                    },
                    new Word
                    {
                        Text = "PIZZA",
                        Difficulty = "Easy",
                        Language = "en",
                        CategoryId = 2, // Food
                        CreatedBy = "System",
                        IsActive = true
                    },
                    new Word
                    {
                        Text = "FRANCE",
                        Difficulty = "Medium",
                        Language = "en",
                        CategoryId = 3, // Countries
                        CreatedBy = "System",
                        IsActive = true
                    }
                };

                foreach (Word word in words)
                {
                    context.Words.Add(word);
                }
                context.SaveChanges();
            }

            // اضافه کردن تنظیمات سیستم
            var systemSettings = new SystemSettings
            {
                MaxDailyGames = 5,
                DefaultDifficulty = "medium",
                EnableGuestPlay = true,
                MaintenanceMode = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.SystemSettings.Add(systemSettings);

            // ذخیره تغییرات
            context.SaveChanges();
        }
    }
} 