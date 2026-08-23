using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Services.Abstractions.Generic;

public interface IModifyService<TCreateDto, TUpdateDto>
    where TCreateDto : IDto
    where TUpdateDto : IDto
{
    Task<int> CreateAsync(TCreateDto dto);
    Task UpdateAsync(TUpdateDto dto);
    Task DeleteAsync(int id);
}
