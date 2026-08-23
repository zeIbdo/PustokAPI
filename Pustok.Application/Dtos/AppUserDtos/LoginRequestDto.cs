using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.AppUserDtos;

public class LoginRequestDto:IDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
