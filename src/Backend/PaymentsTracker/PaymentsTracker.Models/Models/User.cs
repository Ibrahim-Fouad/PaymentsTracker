using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentsTracker.Models.Models;

public class User
{
    public int UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string NormalizedEmail { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public ICollection<Customer> Customers { get; set; } = null!;
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.FullName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.NormalizedEmail)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.PasswordHash)
            .IsRequired();

        builder.Property(p => p.PasswordSalt)
            .IsRequired();
    }
}