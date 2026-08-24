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
        CreateMap<ServiceUpdateDto, Service>().ReverseMap().ForAllMembers(opts => opts.PreCondition((src, dest, srcMember) => srcMember != null));
    }
}
