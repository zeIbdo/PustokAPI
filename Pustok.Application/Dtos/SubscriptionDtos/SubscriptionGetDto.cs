using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.SubscriptionDtos;

public class SubscriptionGetDto:IDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
}
