using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.CategoryDtos;

public class CategoryUpdateDto:IDto
{
    public string? Name { get; set; } 
    public int? ParentId { get; set; }
}
