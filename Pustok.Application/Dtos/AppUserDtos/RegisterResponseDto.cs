namespace Pustok.Application.Dtos.AppUserDtos;

public class RegisterResponseDto
{
    public string Email { get; set; } = string.Empty;
    public IEnumerable<string>? Errors { get; set; }
}
