namespace Pustok.Application.Dtos.ProductDtos;

public class ProductDetailDto
{
    public ProductGetDto Product { get; set; } = null!;
    public List<ProductGetDto> RelatedProducts { get; set; } = [];

}
