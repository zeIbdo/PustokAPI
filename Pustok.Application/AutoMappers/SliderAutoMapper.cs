using AutoMapper;
using Pustok.Application.Dtos.SliderDtos;
using Pustok.Application.Dtos.TagDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class SliderAutoMapper : Profile
{
    public SliderAutoMapper()
    {
        CreateMap<Slider, SliderGetDto>().ReverseMap();
        CreateMap<Slider, SliderCreateDto>().ReverseMap();
        CreateMap<SliderUpdateDto, Slider>().ReverseMap().ForAllMembers(opts => opts.PreCondition((src, dest, srcMember) => srcMember != null));
    }
}
