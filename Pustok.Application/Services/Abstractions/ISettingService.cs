using Pustok.Application.Dtos.SettingDtos;
using Pustok.Application.Services.Abstractions.Generic;

namespace Pustok.Application.Services.Abstractions;

public interface ISettingService:IGetService<SettingGetDto>,IModifyService<SettingCreateDto,SettingUpdateDto>
{
}
