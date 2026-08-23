using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.ProductImageDtos;

public class ProductImageGetDto:IDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public bool IsMain { get; set; }
}   
