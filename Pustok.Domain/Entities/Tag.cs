using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class Tag:BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<ProductTag> ProductTags { get; set; } = [];
}
