using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class Slider : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ImageUrl { get; set; } 
    public decimal Price { get; set; }
}
