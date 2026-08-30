using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.AppUserDtos;

public class RegisterRequestDto : IDto
{
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}
