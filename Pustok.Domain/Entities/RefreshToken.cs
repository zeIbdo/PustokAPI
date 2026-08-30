using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class RefreshToken:BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public string AppUserId { get; set; } = string.Empty;
    public AppUser AppUser { get; set; } = null!;
    public DateTime? RevokedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? ReplacedByToken { get; set; }
    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;
}
