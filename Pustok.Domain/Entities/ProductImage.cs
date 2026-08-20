using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class ProductImage: BaseEntity
{
    public string ImageUrl { get; set; } = null!;
    public bool IsMain { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
