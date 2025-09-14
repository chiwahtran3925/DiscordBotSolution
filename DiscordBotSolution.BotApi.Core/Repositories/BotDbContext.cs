using Microsoft.EntityFrameworkCore;
using DiscordBotSolution.BotApi.Core.Models;

namespace DiscordBotSolution.BotApi.Core.Repositories;
public class BotDbContext : DbContext
{
    public BotDbContext(DbContextOptions<BotDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserTimer> UserTimers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.IsBlocked).IsRequired();
        });

        modelBuilder.Entity<UserTimer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.Duration).IsRequired();
            entity.Property(e => e.Completed).IsRequired();

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(t => t.UserId);
        });
    }
}
