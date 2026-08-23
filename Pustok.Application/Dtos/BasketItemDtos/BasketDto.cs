using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.BasketItemDtos;

public class BasketDto:IDto
{
    public ICollection<BasketItemGetDto> Items { get; set; } = [];
    public decimal Total { get; set; }
    public decimal DiscountedTotal { get; set; }
    public int Count { get; set; }
}
