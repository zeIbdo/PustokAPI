using AutoMapper;
using Pustok.Application.Dtos.SubscriptionDtos;
using Pustok.Application.Dtos.TagDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class SubscriptionAutoMapper : Profile
{
    public SubscriptionAutoMapper()
    {
        CreateMap<Subscription, SubscriptionGetDto>().ReverseMap();
        CreateMap<Subscription, SubscriptionCreateDto>().ReverseMap();
        CreateMap<SubscriptionUpdateDto, Subscription>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
