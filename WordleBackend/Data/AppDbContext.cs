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

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Word> Words { get; set; } = null!;
        public DbSet<Score> Scores { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; } = null!;
        public DbSet<SystemSettings> SystemSettings { get; set; }
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Leaderboard> Leaderboards { get; set; } = null!;
        public DbSet<DailyChallenge> DailyChallenges { get; set; } = null!;
        public DbSet<DailyChallengeParticipation> DailyChallengeParticipations { get; set; } = null!;

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

            // Leaderboard relationships
            modelBuilder.Entity<Leaderboard>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId);

            // Leaderboard configurations
            modelBuilder.Entity<Leaderboard>()
                .HasIndex(l => new { l.Period, l.StartDate, l.EndDate });

            // Daily Challenge configurations
            modelBuilder.Entity<DailyChallenge>()
                .HasIndex(dc => dc.Date)
                .IsUnique();

            modelBuilder.Entity<DailyChallenge>()
                .HasOne(dc => dc.Word)
                .WithMany()
                .HasForeignKey(dc => dc.WordId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DailyChallenge>()
                .HasMany(dc => dc.Participations)
                .WithOne(p => p.Challenge)
                .HasForeignKey(p => p.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DailyChallengeParticipation>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DailyChallengeParticipation>()
                .Property(p => p.Guesses)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
} 