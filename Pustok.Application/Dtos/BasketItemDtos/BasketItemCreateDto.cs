using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.BasketItemDtos;

public class BasketItemCreateDto:IDto
{
    public int Count { get; set; } = 1;
}
