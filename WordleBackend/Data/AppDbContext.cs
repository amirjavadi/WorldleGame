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
        public DbSet<GameHistory> GameHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<GameHistory>()
                .HasOne(gh => gh.User)
                .WithMany(u => u.GameHistories)
                .HasForeignKey(gh => gh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Word>()
                .HasIndex(w => w.Text)
                .IsUnique();
        }
    }
} 