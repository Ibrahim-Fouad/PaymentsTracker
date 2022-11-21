using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PaymentsTracker.Common.Helpers;
using PaymentsTracker.Models.Conversions;
using PaymentsTracker.Models.Models;

namespace PaymentsTracker.Models.Data;

public class AppDbContext : DbContext
{
    private readonly IUserClaimsService _userClaimsService;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options, IUserClaimsService userClaimsService) : base(options)
    {
        _userClaimsService = userClaimsService;
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

        if (_userClaimsService.UserId.HasValue)
        {
            ApplyGlobalQueryFilter(modelBuilder, _userClaimsService.UserId.Value);
        }
    }

    private static void ApplyGlobalQueryFilter(ModelBuilder modelBuilder, int userId)
    {
        modelBuilder.Entity<Customer>()
            .HasQueryFilter(c => c.UserId == userId);
    }
}