using AutoMapper;

namespace PaymentsTracker.Common.Mapping;

public static class CommonMapperConfiguration
{
    public static MapperConfiguration Configuration => new(options =>
        options.AddMaps(typeof(UserMapping).Assembly));
}