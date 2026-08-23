using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.ServiceDtos;

public class ServiceGetDto:IDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? IconUrl { get; set; }
}
