using Microsoft.AspNetCore.Http;
using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.ServiceDtos;

public class ServiceUpdateDto : IDto
{
    public string? Title { get; set; } 
    public string? Description { get; set; } 
    public IFormFile? Image { get; set; } 
}