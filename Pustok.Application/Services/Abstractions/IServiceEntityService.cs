using Pustok.Application.Dtos.ServiceDtos;
using Pustok.Application.Services.Abstractions.Generic;

namespace Pustok.Application.Services.Abstractions;

public interface IServiceEntityService:IGetService<ServiceGetDto>,IModifyService<ServiceCreateDto,ServiceUpdateDto>
{
}
