using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Price { get; set; }
    public decimal Discount { get; set; } = 0;
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int ViewCount { get; set; }
    public string ProductCode { get; set; } = null!;
    public decimal? RatingStar { get; set; }
    public string CreatedBy { get; set; } = null!;
    public string UpdatedBy { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ProductTag> ProductTags { get; set; } = [];
    public ICollection<BasketItem> BasketItems { get; set; } = [];
}
