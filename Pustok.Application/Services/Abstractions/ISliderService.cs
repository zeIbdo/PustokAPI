using Pustok.Application.Dtos.SliderDtos;
using Pustok.Application.Services.Abstractions.Generic;

namespace Pustok.Application.Services.Abstractions;

internal interface ISliderService: IGetService<SliderGetDto>, IModifyService<SliderCreateDto, SliderUpdateDto>
{
}
