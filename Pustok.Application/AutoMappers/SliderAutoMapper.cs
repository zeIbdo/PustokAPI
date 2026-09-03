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
        CreateMap<SliderUpdateDto, Slider>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
        {
            var propertyName = opts.DestinationMember.Name;

            var dtoProperty = src.GetType().GetProperty(propertyName);
            if (dtoProperty != null)
            {
                var rawValue = dtoProperty.GetValue(src);

                if (rawValue == null) return false;
                if (rawValue is string str && string.IsNullOrEmpty(str)) return false;
            }

            return true;
        }));
    }
}
