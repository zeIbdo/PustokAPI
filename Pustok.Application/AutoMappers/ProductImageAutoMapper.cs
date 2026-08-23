using AutoMapper;
using Pustok.Application.Dtos.ProductImageDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class ProductImageAutoMapper : Profile
{
    public ProductImageAutoMapper()
    {
        CreateMap<ProductImage, ProductImageGetDto>().ReverseMap();
    }
}
