using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.SliderDtos;

public class SliderGetDto : IDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
}
