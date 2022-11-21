using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsTracker.API.Extensions;
using PaymentsTracker.Common.DTOs.Auth;
using PaymentsTracker.Services.Interfaces;

namespace PaymentsTracker.API.Controllers;

[AllowAnonymous]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginDto))]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto, CancellationToken token)
    {
        return await _authService.LoginAsync(loginDto, token).ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto, CancellationToken token)
    {
        return await _authService.RegisterAsync(registerDto, token).ToActionResult();
    }
}