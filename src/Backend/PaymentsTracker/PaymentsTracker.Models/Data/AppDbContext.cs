using Microsoft.EntityFrameworkCore;
using PaymentsTracker.Common.Helpers;
using PaymentsTracker.Models.Conversions;
using PaymentsTracker.Models.Models;

namespace PaymentsTracker.Models.Data;

public class AppDbContext : DbContext
{
    private readonly int? _userId;

    public AppDbContext(DbContextOptions<AppDbContext> options, IUserIdService userIdService) : base(options)
    {
        _userId = userIdService.UserId;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetDeclaredProperties().Where(p =>
                         p.ClrType == typeof(DateTimeOffset)))
            {
                if (property.Name.EndsWith("utc", StringComparison.CurrentCultureIgnoreCase))
                    property.SetValueConverter(DateTimeOffsetTypeConversion.DateTimeOffsetUtcValueConverter);
            }
        }

        modelBuilder.Entity<Customer>()
            .HasQueryFilter(c => c.UserId == _userId);
    }
}