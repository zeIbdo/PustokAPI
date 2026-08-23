using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.CategoryDtos;

public class CategoryCreateDto:IDto
{
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; }
}
