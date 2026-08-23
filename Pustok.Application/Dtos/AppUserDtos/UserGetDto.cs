using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.AppUserDtos;

public class UserGetDto : IDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public ICollection<BasketItemGetDto> BasketItems { get; set; } = [];
}

