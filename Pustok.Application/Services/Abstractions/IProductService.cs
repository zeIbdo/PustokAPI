using Pustok.Application.Dtos.ProductDtos;
using Pustok.Application.Services.Abstractions.Generic;

namespace Pustok.Application.Services.Abstractions;

public interface IProductService:IGetService<ProductGetDto>,IModifyService<ProductCreateDto,ProductUpdateDto>
{
}
