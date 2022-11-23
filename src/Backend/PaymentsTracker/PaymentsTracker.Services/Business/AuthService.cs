using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaymentsTracker.Common.DTOs;
using PaymentsTracker.Common.DTOs.Auth;
using PaymentsTracker.Common.Enums;
using PaymentsTracker.Common.Helpers;
using PaymentsTracker.Common.Options;
using PaymentsTracker.Models.Models;
using PaymentsTracker.Repositories.Interfaces;
using PaymentsTracker.Services.Interfaces;

namespace PaymentsTracker.Services.Business;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtOptions _jwtOptions;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IOptions<JwtOptions> jwtOptions, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<OperationResult<UserLoginDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetAsNoTrackingAsync(u => u.Email == loginDto.Email,
            cancellationToken: cancellationToken);
        if (user is null)
            return ErrorDto.Factory(ErrorCode.InvalidEmailOrPassword);

        if (Sha512PasswordManager.ValidatePassword(loginDto.Password, user.PasswordHash, user.PasswordSalt) is false)
            return ErrorDto.Factory(ErrorCode.InvalidEmailOrPassword);
        var result = _mapper.Map<UserLoginDto>(user);
        result.Token = GenerateToken(user);
        return result;
    }

    public async Task<OperationResult<UserLoginDto>> RegisterAsync(RegisterDto registerDto,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = NormalizeEmail(registerDto.Email);
        if (await _unitOfWork.Users.AnyAsync(a => a.NormalizedEmail.ToUpper() == normalizedEmail.ToUpper(),
                cancellationToken))
            return ErrorDto.Factory(ErrorCode.EmailIsAlreadyExists);

        var (passwordHash, passwordSalt) = Sha512PasswordManager.Generate(registerDto.Password);
        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.NormalizedEmail = normalizedEmail;
        _unitOfWork.Users.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        // TODO: Send activation email.
        var result = _mapper.Map<UserLoginDto>(user);
        result.Token = GenerateToken(user);
        return result;
    }

    #region Helper methods

    private static string NormalizeEmail(string email)
    {
        if (email.ToLowerInvariant().EndsWith("@gmail.com") is false) return email;
        var namePart = email.Split("@")[0].Replace(".", string.Empty);
        return $"{namePart}@gmail.com";
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
        };
        var expirationDate = DateTime.Now.Add(_jwtOptions.TokenPeriod);
        return GenerateToken(claims, _jwtOptions.SecretKey, expirationDate);
    }

    private static string GenerateToken(IEnumerable<Claim> claims, string securityKey, DateTime expirationDate)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = expirationDate
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    #endregion
}