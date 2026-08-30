using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Pustok.Application.Dtos.AppUserDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Domain.Enums;
using Pustok.Infrastructure.Repositories.Abstractions;

namespace Pustok.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(UserManager<AppUser> userManager, IJwtService jwtService, IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async Task LogoutAsync(string token)
    {
        await _jwtService.RevokeAsync(token);
    }
    public async Task<LoginResponseDto> RefreshAsync(RefreshRequest refreshRequest)
    {
        var existingToken = await _refreshTokenRepository.GetAsync(x => x.Token == refreshRequest.RefreshToken);
        if (existingToken == null)
            throw new UnauthorizedException("Invalid credential");
        if (!existingToken.IsActive)
        {
            if (existingToken.RevokedAt != null)
                await _jwtService.RevokeAllActiveTokensAsync(existingToken.AppUserId);

            throw new UnauthorizedException("Invalid credential");
        }
        var user = await _userManager.FindByIdAsync(existingToken.AppUserId);
        if(user  == null)
            throw new UnauthorizedException("Invalid credential");
        var (newRefToken, refreshExpiresAt) = await _jwtService.CreateRefreshTokenAsync(user.Id);
        existingToken.RevokedAt = DateTime.UtcNow;
        existingToken.ReplacedByToken = newRefToken;
        var newEntity = new RefreshToken
        {
            AppUserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = refreshExpiresAt,
            Token = newRefToken
        };
        await _refreshTokenRepository.CreateAsync(newEntity);
        await _refreshTokenRepository.SaveChangesAsync();
        var roles = await _userManager.GetRolesAsync(user);
        var (accessToken, accessExpiresAt) = _jwtService.CreateToken(user, roles);
        return new LoginResponseDto
        {
            Email = user.Email!,
            Id = user.Id,
            ExpiresAt = accessExpiresAt,
            Roles = roles,
            Token = accessToken,
            RefreshToken = newRefToken,
            RefreshExpiresAt = refreshExpiresAt 
        };
    }

    public async Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto dto)
    {
        if (await _userManager.Users.AnyAsync(x => x.NormalizedEmail == dto.Email.ToUpper()))
            throw new AlreadyExistsException("This email is already in use");
        if (await _userManager.Users.AnyAsync(x => x.NormalizedUserName == dto.Username.ToUpper()))
            throw new AlreadyExistsException("This username is already in use");
        var newUser = new AppUser
        {
            UserName = dto.Username,
            Email = dto.Email,
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(newUser, dto.Password);
        var registerResponse = new RegisterResponseDto();
        if (!result.Succeeded)
        {
            registerResponse.Errors =result.Errors.Select(e => e.Description);
            return registerResponse;
        }
        await _userManager.AddToRoleAsync(newUser, IdentityRoles.Member.ToString());
        registerResponse.Email = newUser.Email;
        return registerResponse;
    }

    public async Task<LoginResponseDto> SignInUserAsync(LoginRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new UnauthorizedException("Invalid infos");
        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedException("Invalid infos");
        var roles = await _userManager.GetRolesAsync(user);
        var (token, expiresAt) = _jwtService.CreateToken(user, roles);
        var (rToken, rExpiresAt) = await _jwtService.CreateRefreshTokenAsync(user.Id);
        return new LoginResponseDto
        {
            Email = user.Email!,
            Id = user.Id,
            ExpiresAt = expiresAt,
            Roles = roles,
            Token = token,
            RefreshToken = rToken,
            RefreshExpiresAt = rExpiresAt
        };
    }
}
