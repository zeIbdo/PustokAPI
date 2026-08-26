using Pustok.Application.Dtos.SliderDtos;
using Pustok.Application.Services.Abstractions.Generic;

namespace Pustok.Application.Services.Abstractions;

public interface ISliderService: IGetService<SliderGetDto>, IModifyService<SliderCreateDto, SliderUpdateDto>
{
}
