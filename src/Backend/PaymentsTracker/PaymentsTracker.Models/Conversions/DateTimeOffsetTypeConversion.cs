using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PaymentsTracker.Models.Conversions;

public static class DateTimeOffsetTypeConversion
{
    public static ValueConverter DateTimeOffsetUtcValueConverter =>
        new ValueConverter<DateTimeOffset, DateTimeOffset>(toDb => toDb.ToUniversalTime(), fromDb => fromDb);
}