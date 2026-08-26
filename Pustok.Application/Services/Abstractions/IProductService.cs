using Pustok.Application.Dtos.ProductDtos;
using Pustok.Application.Services.Abstractions.Generic;
using Pustok.Infrastructure.Paging;

namespace Pustok.Application.Services.Abstractions;

public interface IProductService:IGetService<ProductGetDto>,IModifyService<ProductCreateDto,ProductUpdateDto>
{
    Task<List<ProductGetDto>> GetProductsByCategory(int categoryId);
    Task<List<ProductGetDto>> GetNewestProducts();
    Task<List<ProductGetDto>> GetDiscountedProducts();
}
