using Microsoft.EntityFrameworkCore;
using PaymentsTracker.Models.Conversions;

namespace PaymentsTracker.Models.Data;

public class AppDbContext : DbContext
{
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
                         p.ClrType == typeof(DateTimeOffset) &&
                         p.Name.EndsWith("utc", StringComparison.CurrentCultureIgnoreCase)))
            {
                property.SetValueConverter(DateTimeOffsetTypeConversion.DateTimeOffsetUtcValueConverter);
            }
        }
    }
}