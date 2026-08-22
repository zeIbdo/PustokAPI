using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Discount { get; set; } = 0;
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int ViewCount { get; set; } = 0;
    public string ProductCode { get; set; } = null!;
    public decimal? RatingStar { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; } = [];
    public ICollection<BasketItem> BasketItems { get; set; } = [];
    public ICollection<ProductImage> ProducImages { get; set; } = [];
}
