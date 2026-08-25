using Pustok.Application.Dtos.TagDtos;
using Pustok.Application.Services.Abstractions.Generic;
using Pustok.Domain.Entities;

namespace Pustok.Application.Services.Abstractions;

public interface ITagService:IGetService<TagGetDto>,IModifyService<TagCreateDto,TagUpdateDto>
{
}
