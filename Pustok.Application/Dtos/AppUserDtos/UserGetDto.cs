using Pustok.Application.Dtos.BasketItemDtos;
using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.AppUserDtos;

public class UserGetDto : IDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public IList<string> Roles { get; set; } = [];
}

    