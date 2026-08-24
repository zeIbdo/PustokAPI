using AutoMapper;
using Pustok.Application.Dtos.ProductDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class ProductAutoMapper : Profile
{
    public ProductAutoMapper()
    {
        CreateMap<Product, ProductGetDto>().ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag))).ReverseMap();
        CreateMap<ProductCreateDto, Product>().ForMember(dest => dest.ProductTags, opt => opt.MapFrom(src => src.TagIds.Select(ti => new ProductTag { TagId = ti }))).ReverseMap();
        CreateMap<ProductUpdateDto, Product>()
            .ForMember(dest => dest.ProductTags,
                opt => opt.MapFrom(src => src.TagIds != null ? src.TagIds.Select(ti => new ProductTag { TagId = ti }).ToList() : null))
            .ForAllMembers(opts => opts.PreCondition((src, dest, srcMember) => srcMember != null));
    }
}