using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace FlappyBackend.Infrastructure.Persistence
{
    public class FlappyDbContext : DbContext
    {
        public FlappyDbContext(DbContextOptions<FlappyDbContext> options) : base(options) { }

        public DbSet<Score> Scores => Set<Score>();
        public DbSet<Alias> Aliases => Set<Alias>();
        public DbSet<Session> Sessions => Set<Session>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Score>(entity =>
            {
                entity.ToTable("Score");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Alias).IsRequired().HasMaxLength(30);
                entity.Property(e => e.Points).IsRequired();
                entity.Property(e => e.MaxCombo);
                entity.Property(e => e.DurationSec);
                entity.Property(e => e.Metadata).HasMaxLength(400);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                entity.HasIndex(e => e.Points).HasDatabaseName("IX_Score_Points_DESC").IsDescending(true);
                entity.HasOne(x => x.Session)
                      .WithMany()
                      .HasForeignKey(e => e.SessionId)
                      .HasConstraintName("FK_Score_SessionId")
                      .IsRequired(false);
            });

            modelBuilder.Entity<Alias>(entity =>
            {
                entity.ToTable("Alias");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(30).HasColumnName("Name");
                entity.HasIndex(e => e.Name).IsUnique().HasDatabaseName("UQ_Alias_Name");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Session");
                entity.HasKey(e => e.SessionId);
                entity.Property(e => e.Alias).HasMaxLength(30);
                entity.Property(e => e.StartedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");
                entity.Property(e => e.EndedAt);
                entity.Property(e => e.Metadata).HasMaxLength(400);
            });
        }
    }
}
