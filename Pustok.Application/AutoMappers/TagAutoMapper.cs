using AutoMapper;
using Pustok.Application.Dtos.TagDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

internal class TagAutoMapper : Profile
{
    public TagAutoMapper()
    {
        CreateMap<Tag, TagGetDto>().ReverseMap();
        CreateMap<Tag, TagCreateDto>().ReverseMap();
        CreateMap<Tag, TagUpdateDto>().ReverseMap();
    }
}
