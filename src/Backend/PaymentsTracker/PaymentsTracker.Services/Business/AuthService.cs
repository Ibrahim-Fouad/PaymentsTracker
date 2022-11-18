using PaymentsTracker.Common.DTOs;
using PaymentsTracker.Common.DTOs.Auth;
using PaymentsTracker.Repositories.Interfaces;
using PaymentsTracker.Services.Interfaces;

namespace PaymentsTracker.Services.Business;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<OperationResult<UserLoginDto>> LoginAsync(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<UserLoginDto>> RegisterAsync(RegisterDto registerDto)
    {
        throw new NotImplementedException();
    }
}