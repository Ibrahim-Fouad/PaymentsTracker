using AutoMapper;
using PaymentsTracker.Common.DTOs.Auth;
using PaymentsTracker.Models.Models;

namespace PaymentsTracker.Mappers;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserLoginDto>();
        CreateMap<RegisterDto, User>()
            .ForMember(a => a.CreatedAtUtc, dest => dest.MapFrom(x => DateTimeOffset.UtcNow));
    }
}