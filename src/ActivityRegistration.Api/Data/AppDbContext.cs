using Microsoft.EntityFrameworkCore;
using ActivityRegistration.Api.Entities;

namespace ActivityRegistration.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Activity> Activities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(100);
            entity.Property(x => x.Category).HasMaxLength(50);
        });
    }
}

