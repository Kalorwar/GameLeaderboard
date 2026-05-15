using GameLeaderboard.Domain.Entities;
using GameLeaderboard.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameLeaderboard.Infrastructure.Persistence;

public class ScoreRecordConfiguration : IEntityTypeConfiguration<ScoreRecord>
{
    public void Configure(EntityTypeBuilder<ScoreRecord> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Points).IsRequired();

        builder.HasOne(r => r.User).WithMany(u => u.Scores).HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.Points).IsDescending();
        builder.HasIndex(r => r.UserId);
    }
}

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ScoreRecord> Scores { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}