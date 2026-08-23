using AutoMapper;
using Pustok.Application.Dtos.CategoryDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class CategoryAutoMapper : Profile
{
    public CategoryAutoMapper()
    {
        CreateMap<Category, CategoryGetDto>().ReverseMap();
        CreateMap<Category, CategoryCreateDto>().ReverseMap();
        CreateMap<Category, CategoryUpdateDto>().ReverseMap();
    }
}
