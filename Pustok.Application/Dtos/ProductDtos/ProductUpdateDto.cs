using Microsoft.AspNetCore.Http;
using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.ProductDtos;

public class ProductUpdateDto:IDto
{
    public string? Name { get; set; } 
    public string? Description { get; set; } 
    public decimal? Price { get; set; }
    public decimal? Discount { get; set; } 
    public int? Stock { get; set; }
    public int? CategoryId { get; set; }
    public string? ProductCode { get; set; } 
    public ICollection<int>? TagIds { get; set; } 
    public ICollection<IFormFile>? AdditionalImages { get; set; } 
    public IFormFile? MainImage { get; set; } 
}
