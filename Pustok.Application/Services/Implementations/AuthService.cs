using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pustok.Application.Dtos.AppUserDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Domain.Enums;
using Pustok.Infrastructure.Repositories.Abstractions;
using System.Threading.Tasks;

namespace Pustok.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public AuthService(UserManager<AppUser> userManager, IJwtService jwtService, IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration, IMapper mapper, IEmailService emailService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
        _mapper = mapper;
        _emailService = emailService;
    }


    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return false;
        if (await _userManager.CheckPasswordAsync(user, dto.Password)) throw new SamePasswordException();
        var token = Uri.UnescapeDataString(dto.Token);
        var result = await _userManager.ResetPasswordAsync(user,token,dto.ConfirmPassword);
        if(!result.Succeeded)
            return false;
        return true;
    }

    public async Task<bool> SendResetTokenToEmailAsync(ForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return false;
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);
        var resetLink = $"website/reset-password?email={dto.Email}&token={encodedToken}";
        var subject = "Reset Password with this link";
        var body = $"<b>Password Reset Token</b><br><hr> <a href=\"{resetLink}\">Recovery Link</a> ";
        _emailService.SendEmail(dto.Email, subject, body);
        return true;
    }


    public async Task ChangeRoleAsync(string id, AppUserRoleChangeDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new NotFoundException("User not found");
        if (!Enum.TryParse<IdentityRoles>(dto.Role, true, out var parsedRole) || parsedRole == IdentityRoles.Admin)
            throw new RoleAssignException("Invalid role");
        if (await _userManager.IsInRoleAsync(user, parsedRole.ToString()))
            throw new RoleAssignException("Already in this role");
        var userRoles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
        if (!result.Succeeded)
            throw new RoleAssignException("Error happened in role assign process");
        result = await _userManager.AddToRoleAsync(user, parsedRole.ToString());
        if (!result.Succeeded)
            throw new RoleAssignException("Error happened in role assign process");
    }

    public async Task<bool> ChangeStatusAsync(string userId, AppUserStatusChangeDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found");
        if (user.IsDisabled == dto.MakeDisable)
            throw new SameAppUserStatusException();
        user.IsDisabled = dto.MakeDisable;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return false;
        return true;
    }

    public async Task<List<UserGetDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var admin = users.FirstOrDefault(x => x.Email == _configuration["AdminAdam:Email"]);
        if (admin != null) users.Remove(admin);
        var userDtos = _mapper.Map<List<UserGetDto>>(users);
        foreach (var dto in userDtos)
        {
            var user = users.First(x => x.Id == dto.Id);
            var roles = await _userManager.GetRolesAsync(user!);
            dto.Roles = roles;
        }
        return userDtos;
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
        if (user == null)
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
            registerResponse.Errors = result.Errors.Select(e => e.Description);
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
