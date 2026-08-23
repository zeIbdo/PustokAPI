using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.TagDtos;

public class TagCreateDto : IDto
{
    public string Name { get; set; } = null!;
}
