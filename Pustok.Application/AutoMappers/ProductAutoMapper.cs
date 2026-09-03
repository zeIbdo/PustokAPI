using AutoMapper;
using Pustok.Application.Dtos.ProductDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class ProductAutoMapper : Profile
{
    public ProductAutoMapper()
    {
        CreateMap<Product, ProductGetDto>().ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag))).ReverseMap();
        CreateMap<ProductCreateDto, Product>().ForMember(dest => dest.ProductTags, opt => opt.Ignore());

        CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
                    .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
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