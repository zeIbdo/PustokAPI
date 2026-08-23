using Microsoft.AspNetCore.Http;
using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.ServiceDtos;

public class ServiceCreateDto : IDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile Image { get; set; } = null!;
}
