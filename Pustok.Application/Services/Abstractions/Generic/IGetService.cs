using Pustok.Application.Dtos.Generic;
using Pustok.Infrastructure.Paging;

namespace Pustok.Application.Services.Abstractions.Generic;

public interface IGetService<TGetDto> where TGetDto : IDto
{
    Task<TGetDto> GetAsync(int id);

    Task<List<TGetDto>> GetAllAsync();

    Task<Paginate<TGetDto>> GetPaginateAsync(int index = 0, int size = 10);
}
