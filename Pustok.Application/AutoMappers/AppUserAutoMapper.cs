using AutoMapper;
using Pustok.Application.Dtos.AppUserDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.AutoMappers;

public class AppUserAutoMapper : Profile
{
    public AppUserAutoMapper()
    {
        CreateMap<AppUser, UserGetDto>().ReverseMap();
    }
}