using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentsTracker.Models.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? SecondPhone { get; set; }
    public string? Description { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable($"{nameof(Customer)}s");
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(c => c.SecondPhone)
            .IsRequired(false)
            .HasMaxLength(15);

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(500);
    }
}