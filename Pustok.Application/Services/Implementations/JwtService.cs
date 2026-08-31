using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Pustok.Application.AppSettingModels;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Repositories.Abstractions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pustok.Application.Services.Implementations;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public JwtService(IOptions<JwtSettings> jwtSettings, IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async Task RevokeAllActiveTokensAsync(string userId)
    {
        var activeTokens = await _refreshTokenRepository.GetAll(predicate: x => x.AppUserId == userId && x.RevokedAt == null).ToListAsync();
        foreach (var token in activeTokens)
        {
            token.RevokedAt = DateTime.UtcNow;
        }

        await _refreshTokenRepository.SaveChangesAsync();
    }
    public async Task RevokeAsync(string token)
    {
        var existingToken = await _refreshTokenRepository.GetAsync(x=>x.Token==token);
        if (existingToken == null)
            throw new NotFoundException();
        if (!existingToken.IsActive)
            throw new AlreadyExistsException("Already not active");
        existingToken.RevokedAt = DateTime.UtcNow;
        _refreshTokenRepository.Update(existingToken);
        await _refreshTokenRepository.SaveChangesAsync();
    }
    public async Task<(string Token, DateTime ExpiresAt)> CreateRefreshTokenAsync(string userId)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(32);
        var tokenText = WebEncoders.Base64UrlEncode(randomBytes);
        var refreshExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
        var rToken = new RefreshToken
        {
            AppUserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = refreshExpiresAt,
            Token = tokenText
        };
        await _refreshTokenRepository.CreateAsync(rToken);
        await _refreshTokenRepository.SaveChangesAsync();
        return (tokenText, refreshExpiresAt);
    }

    public (string Token, DateTime ExpiresAt) CreateToken(AppUser user, IEnumerable<string> roles)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,user.Id),
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email,user.Email ?? "email"),
            new(JwtRegisteredClaimNames.Name,user.UserName ?? "username")
        };
        claims.AddRange(roles.Select(role => new Claim("role", role)));
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.JwtSecret));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var descriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            Expires = expiresAt,
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(claims)
        };
        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(descriptor);
        return (token, expiresAt);
    }
}
