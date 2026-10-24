using Microsoft.EntityFrameworkCore;
using ActivityRegistration.Api.Entities;

namespace ActivityRegistration.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ActivityEntity> Activities { get; set; }
    public DbSet<RegistrationEntity> Registrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityEntity>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(100);
            entity.Property(x => x.Category).HasMaxLength(50);

            entity.HasData(new ActivityEntity { Id = 1, Title = "技術分享", Category = "講座", Capacity = 30 });
            entity.HasData(new ActivityEntity { Id = 2, Title = "下班羽球", Category = "運動", Capacity = 16 });
            entity.HasData(new ActivityEntity { Id = 3, Title = "咖啡沖煮", Category = "手作", Capacity = 12 });
        });



        modelBuilder.Entity<RegistrationEntity>(entity =>
        {
            entity.Property(x => x.EmployeeId).HasMaxLength(450);

            entity.HasIndex(x => new { x.ActivityId, x.EmployeeId }).IsUnique();   // 一人一活動只能有一筆

            entity.HasOne(x => x.Activity)
                .WithMany(x => x.Registrations)
                .HasForeignKey(x => x.ActivityId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // 所有資料表統一加上專案前綴 EAR_（Employee Activity Registration）；放在最後，新增的 Entity 也會自動套用
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.SetTableName($"EAR_{entityType.GetTableName()}");
        }
    }
}

