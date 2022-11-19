namespace PaymentsTracker.Common.DTOs.Auth;

public record UserLoginDto(string FullName, string Email)
{
    public string? Token { get; set; }
}