using Pustok.Application.Dtos.ProductDtos;
using Pustok.Application.Services.Abstractions.Generic;
using Pustok.Infrastructure.Paging;

namespace Pustok.Application.Services.Abstractions;

public interface IProductService:IGetService<ProductGetDto>,IModifyService<ProductCreateDto,ProductUpdateDto>
{
    Task<List<ProductGetDto>> GetProductsByCategoryAsync(int categoryId);
    Task<List<ProductGetDto>> GetProductsByNameAsync(string name);
    Task<List<ProductGetDto>> GetNewestProductsAsync();
    Task<List<ProductGetDto>> GetDiscountedProductsAsync();
    Task<List<ProductGetDto>> GetMostSoldProductsAsync();
    Task<ProductDetailDto> GetDetailedProductAsync(int id);
}
