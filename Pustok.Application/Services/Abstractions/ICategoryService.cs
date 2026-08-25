using Pustok.Application.Dtos.CategoryDtos;
using Pustok.Application.Services.Abstractions.Generic;

namespace Pustok.Application.Services.Abstractions;

public interface ICategoryService:IGetService<CategoryGetDto>,IModifyService<CategoryCreateDto,CategoryUpdateDto>
{
}
