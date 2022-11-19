using System.ComponentModel;
using System.Reflection;

namespace PaymentsTracker.Common.Extensions;

public static class EnumerationsEx
{
    public static string GetDescription(this Enum @enum)
    {
        var field = @enum.GetType().GetField(@enum.ToString());
        if (field is null) return @enum.ToString();
        if (field.GetCustomAttribute<DescriptionAttribute>() is not { } descriptionAttribute)
            return @enum.ToString();

        return descriptionAttribute.Description;
    }
}