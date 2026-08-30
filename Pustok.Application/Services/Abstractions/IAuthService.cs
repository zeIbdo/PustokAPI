using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Pustok.Application.Dtos.AppUserDtos;

namespace Pustok.Application.Services.Abstractions;

public interface IAuthService
{
    public Task LogoutAsync(string token);
    Task<LoginResponseDto> RefreshAsync(RefreshRequest refreshRequest);
    Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto dto);
    Task<LoginResponseDto> SignInUserAsync(LoginRequestDto dto);
}
