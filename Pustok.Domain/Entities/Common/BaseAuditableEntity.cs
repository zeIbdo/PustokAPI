namespace Pustok.Domain.Entities.Common;

public abstract class BaseAuditableEntity:BaseEntity
{
    public string CreatedBy { get; set; } = null!;
    public string? UpdatedBy { get; set; } 
    public DateTimeOffset? CreatedAt { get; set; } 
    public DateTimeOffset? UpdatedAt { get; set; } 
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
