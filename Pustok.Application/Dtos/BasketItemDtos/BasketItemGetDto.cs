using Pustok.Application.Dtos.Generic;
using Pustok.Application.Dtos.ProductDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.Dtos.BasketItemDtos;

public class BasketItemGetDto:IDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductGetDto Product { get; set; } = null!;
    public int Count { get; set; }
}
