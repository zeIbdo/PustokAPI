using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.SubscriptionDtos;

public class SubscriptionCreateDto : IDto
{
    public string Email { get; set; } = null!;
}
