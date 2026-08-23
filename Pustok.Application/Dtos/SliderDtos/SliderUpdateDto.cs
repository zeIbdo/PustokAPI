using Microsoft.AspNetCore.Http;
using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.SliderDtos;

public class SliderUpdateDto : IDto
{
    public string? Title { get; set; } 
    public string? Description { get; set; } 
    public IFormFile? Image { get; set; } 
    public decimal? Price { get; set; }
}