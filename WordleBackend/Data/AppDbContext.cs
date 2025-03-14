using Microsoft.EntityFrameworkCore;
using WordleBackend.Models;

namespace WordleBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configurations
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Word configurations
            modelBuilder.Entity<Word>()
                .HasIndex(w => w.Text)
                .IsUnique();

            // Category configurations
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // User relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Scores)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.GameHistories)
                .WithOne(gh => gh.User)
                .HasForeignKey(gh => gh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Word relationships
            modelBuilder.Entity<Word>()
                .HasMany(w => w.GameHistories)
                .WithOne(gh => gh.Word)
                .HasForeignKey(gh => gh.WordId)
                .OnDelete(DeleteBehavior.Cascade);

            // Category relationships
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Words)
                .WithOne(w => w.Category)
                .HasForeignKey(w => w.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 