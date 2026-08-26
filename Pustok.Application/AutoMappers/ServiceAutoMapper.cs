using AutoMapper;
using Pustok.Application.Dtos.ServiceDtos;
using Pustok.Application.Dtos.TagDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class ServiceAutoMapper : Profile
{
    public ServiceAutoMapper()
    {
        CreateMap<Service, ServiceGetDto>().ReverseMap();
        CreateMap<Service, ServiceCreateDto>().ReverseMap();
        CreateMap<ServiceUpdateDto, Service>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
