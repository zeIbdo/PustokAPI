using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.BasketItemDtos;

public class BasketItemUpdateDto:IDto
{
    public int Count { get; set; }
}
