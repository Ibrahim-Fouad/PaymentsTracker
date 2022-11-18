using System.ComponentModel.DataAnnotations;

namespace PaymentsTracker.Common.DTOs.Auth;

public record LoginDto([Required] string Email, [Required] string Password);