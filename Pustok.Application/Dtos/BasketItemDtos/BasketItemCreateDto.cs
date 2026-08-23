using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.BasketItemDtos;

public class BasketItemCreateDto:IDto
{
    public int ProductId { get; set; }
    public int Count { get; set; }
}
