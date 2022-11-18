namespace PaymentsTracker.Common.DTOs.Auth;

public record UserLoginDto(string FullName, string Email, string Token);