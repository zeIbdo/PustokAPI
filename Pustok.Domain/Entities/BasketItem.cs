using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class BasketItem:BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;
    public int Count { get; set; }
}
