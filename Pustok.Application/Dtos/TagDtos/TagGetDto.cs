using Pustok.Application.Dtos.Generic;
using Pustok.Application.Dtos.ProductTagDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.Dtos.TagDtos;

public class TagGetDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<ProductTagGetDto> ProductTags { get; set; } = [];
}
