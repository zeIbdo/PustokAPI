using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.SubscriptionDtos;

public class SubscriptionUpdateDto : IDto
{
    public string? Email { get; set; } 
}