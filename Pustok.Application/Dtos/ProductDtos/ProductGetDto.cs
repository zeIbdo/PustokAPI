using Pustok.Application.Dtos.BasketItemDtos;
using Pustok.Application.Dtos.CategoryDtos;
using Pustok.Application.Dtos.Generic;
using Pustok.Application.Dtos.ProductImageDtos;
using Pustok.Application.Dtos.TagDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.Dtos.ProductDtos;

public class ProductGetDto:IDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Discount { get; set; } = 0;
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public CategoryGetDto Category { get; set; } = null!;
    public int ViewCount { get; set; } = 0;
    public string ProductCode { get; set; } = null!;
    public decimal? RatingStar { get; set; }
    public ICollection<TagGetDto> Tags { get; set; } = [];
    //public ICollection<BasketItemGetDto> BasketItems { get; set; } = [];
    public ICollection<ProductImageGetDto> ProductImages { get; set; } = [];
}
