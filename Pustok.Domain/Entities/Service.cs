using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class Service:BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? IconUrl { get; set; }
}
