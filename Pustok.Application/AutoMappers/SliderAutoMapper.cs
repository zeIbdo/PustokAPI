using AutoMapper;
using Pustok.Application.Dtos.SliderDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class SliderAutoMapper : Profile
{
    public SliderAutoMapper()
    {
        CreateMap<Slider, SliderGetDto>().ReverseMap();
        CreateMap<Slider, SliderCreateDto>().ReverseMap();
        CreateMap<Slider, SliderUpdateDto>().ReverseMap();
    }
}
