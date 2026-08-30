using Pustok.Application.AppSettingModels;
using Pustok.Domain.Entities;

namespace Pustok.Application.Services.Abstractions;

public interface IJwtService
{
    public  Task RevokeAsync(string token);
    public Task RevokeAllActiveTokensAsync(string userId);
    public Task<(string Token, DateTime ExpiresAt)> CreateRefreshTokenAsync(string userId);
    public (string Token, DateTime ExpiresAt) CreateToken(AppUser user, IEnumerable<string> roles);
}
