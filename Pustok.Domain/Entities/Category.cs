using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class Category:BaseEntity
{
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category> Children { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
}
