using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.AppUserDtos;

public class LoginResponseDto:IDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public IList<string> Roles { get; set; } = [];
    public DateTime ExpiresAt { get; set; }
    public DateTime RefreshExpiresAt { get; set; }
}
