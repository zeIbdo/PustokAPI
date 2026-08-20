using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class Subscription:BaseEntity
{
    public string Email { get; set; } = null!;
}
