using System.ComponentModel.DataAnnotations;

namespace PaymentsTracker.Common.DTOs.Auth;

public record RegisterDto([Required] string FullName, [Required] string Email, [Required] string Password);