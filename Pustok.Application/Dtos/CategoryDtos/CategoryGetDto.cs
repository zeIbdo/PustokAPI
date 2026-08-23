using Pustok.Application.Dtos.Generic;
using Pustok.Application.Dtos.ProductDtos;
using Pustok.Domain.Entities;

namespace Pustok.Application.Dtos.CategoryDtos;

public class CategoryGetDto:IDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; }
    public CategoryGetDto? Parent { get; set; }
    public ICollection<CategoryGetDto> Children { get; set; } = [];
    public ICollection<ProductGetDto> Products { get; set; } = [];
}
