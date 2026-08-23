using Microsoft.AspNetCore.Http;
using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.SliderDtos;

public class SliderCreateDto : IDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile Image { get; set; } = null!;
    public decimal Price { get; set; }
}
