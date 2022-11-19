namespace PaymentsTracker.Common.Options;

public class JwtOptions
{
    public const string JwtSectionName = "Jwt";
    public string SecretKey { get; set; } = string.Empty;
    public TimeSpan TokenPeriod { get; set; } = TimeSpan.FromMinutes(30);
}