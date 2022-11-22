using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PaymentsTracker.Common.Helpers;

public interface IUserIdService
{
    public int? UserId { get; }
}

public class UserIdService : IUserIdService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId =>
        int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
            out var userId)
            ? userId
            : null;
}