using AutoMapper;
using Pustok.Application.Dtos.CategoryDtos;
using Pustok.Application.Dtos.TagDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class CategoryAutoMapper : Profile
{
    public CategoryAutoMapper()
    {
        CreateMap<Category, CategoryGetDto>().ReverseMap();
        CreateMap<Category, CategoryCreateDto>().ReverseMap();
        CreateMap<CategoryUpdateDto, Category>().ReverseMap().ForAllMembers(opts => opts.PreCondition((src, dest, srcMember) => srcMember != null));
    }
}
