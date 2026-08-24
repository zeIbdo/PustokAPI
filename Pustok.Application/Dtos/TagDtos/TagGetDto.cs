using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.TagDtos;

public class TagGetDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
