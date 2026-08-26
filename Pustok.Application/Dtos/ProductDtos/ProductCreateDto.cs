using Microsoft.AspNetCore.Http;
using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.ProductDtos;

public class ProductCreateDto:IDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Discount { get; set; } = 0;
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public string ProductCode { get; set; } = null!;
    public ICollection<int>? TagIds { get; set; } 
    public ICollection<IFormFile> AdditionalImages { get; set; } = [];
    public IFormFile MainImage { get; set; } = null!; 
}
