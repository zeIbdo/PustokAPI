using Pustok.Domain.Entities.Common;

namespace Pustok.Domain.Entities;

public class Setting:BaseEntity
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}
