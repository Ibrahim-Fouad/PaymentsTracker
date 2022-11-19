﻿using System.ComponentModel.DataAnnotations;

namespace PaymentsTracker.Common.DTOs.Auth;

public record LoginDto([Required][EmailAddress] string Email, [Required] string Password);