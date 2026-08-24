using AutoMapper;
using Pustok.Application.Dtos.BasketItemDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class BasketItemAutoMapper : Profile
{
    public BasketItemAutoMapper()
    {
        CreateMap<BasketItem, BasketItemGetDto>().ReverseMap();
        CreateMap<BasketItem, BasketItemCreateDto>().ReverseMap();
    }
}
