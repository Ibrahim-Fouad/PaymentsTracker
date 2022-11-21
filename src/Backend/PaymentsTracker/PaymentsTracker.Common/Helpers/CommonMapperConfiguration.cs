using System.Reflection;
using AutoMapper;

namespace PaymentsTracker.Common.Helpers;

public static class CommonMapperConfiguration
{
    public static IEnumerable<Assembly>  LoadedAssemblies => AppDomain.CurrentDomain.GetAssemblies().
        Where(a => a.GetName().Name!.StartsWith(nameof(PaymentsTracker)));

    public static MapperConfiguration Configuration => new(options =>
        options.AddMaps(LoadedAssemblies));

}