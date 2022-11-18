using PaymentsTracker.Common.DTOs;
using PaymentsTracker.Common.DTOs.Auth;

namespace PaymentsTracker.Services.Interfaces;

public interface IAuthService
{
    Task<OperationResult<UserLoginDto>> LoginAsync(LoginDto loginDto);
    Task<OperationResult<UserLoginDto>> RegisterAsync(RegisterDto registerDto);
}