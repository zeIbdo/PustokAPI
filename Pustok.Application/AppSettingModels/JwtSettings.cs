namespace Pustok.Application.AppSettingModels;

public class JwtSettings
{
    public string JwtSecret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; }
    public int RefreshTokenExpiryDays { get; set; } 
}
