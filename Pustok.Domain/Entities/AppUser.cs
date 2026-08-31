using Microsoft.AspNetCore.Identity;

namespace Pustok.Domain.Entities;

public class AppUser:IdentityUser
{
    public bool IsDisabled { get; set; } = false;
    public ICollection<BasketItem> BasketItems { get; set; } = [];
}
