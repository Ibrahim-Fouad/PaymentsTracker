using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PaymentsTracker.Models.Conversions;
using PaymentsTracker.Models.Models;

namespace PaymentsTracker.Models.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetDeclaredProperties().Where(p =>
                         p.ClrType == typeof(DateTimeOffset)
                     ))
            {
                if (property.Name.EndsWith("utc", StringComparison.CurrentCultureIgnoreCase))
                    property.SetValueConverter(DateTimeOffsetTypeConversion.DateTimeOffsetUtcValueConverter);

                if (property.Name.Equals("CreatedAtUtc", StringComparison.CurrentCultureIgnoreCase))
                    property.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            }
        }
    }
}