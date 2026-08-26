using AutoMapper;
using Pustok.Application.Dtos.SettingDtos;
using Pustok.Application.Dtos.TagDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class SettingAutoMapper : Profile
{
    public SettingAutoMapper()
    {
        CreateMap<Setting, SettingGetDto>().ReverseMap();
        CreateMap<Setting, SettingCreateDto>().ReverseMap();
        CreateMap<SettingUpdateDto, Setting>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
