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

            entity.HasData(new Activity { Id = 1, Title = "技術分享", Category = "講座", Capacity = 30 });
            entity.HasData(new Activity { Id = 2, Title = "下班羽球", Category = "運動", Capacity = 16 });
            entity.HasData(new Activity { Id = 3, Title = "咖啡沖煮", Category = "手作", Capacity = 12 });
        });
    }
}

