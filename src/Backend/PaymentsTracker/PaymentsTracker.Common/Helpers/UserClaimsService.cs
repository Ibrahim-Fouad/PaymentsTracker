using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PaymentsTracker.Common.Helpers;

public interface IUserClaimsService
{
    public int? UserId { get; }
}

public class UserClaimsService : IUserClaimsService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserClaimsService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId =>
        int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
            out var userId)
            ? userId
            : null;
}