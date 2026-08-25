using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pustok.Domain.Entities;
using Pustok.Domain.Enums;
using Pustok.Infrastructure.Contexts;
using System.Threading.Tasks;

namespace Pustok.Infrastructure.DataInitializers;

public class DbContextInitializer
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppUser _admin;

    public DbContextInitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _admin = _configuration.GetSection("AdminAdam").Get<AppUser>() ?? new();
    }
    public async Task InitializeDbAsync()
    {
        await _context.Database.MigrateAsync();
        await _addRolesAsync();
        await _addAdminAdamAsync();
    }

    private async Task _addAdminAdamAsync()
    {
        if (await _userManager.Users.AnyAsync(u => u.Email == _admin.Email))
            return;

        var adminPassword = _configuration["AdminAdam:Password"];
        await _userManager.CreateAsync(_admin, adminPassword!);
        await _userManager.AddToRoleAsync(_admin, IdentityRoles.Admin.ToString());
    }
    
    private async Task _addRolesAsync()
    {
        var roles = Enum.GetNames(typeof(IdentityRoles));
        foreach (var role in roles)
        {
            if (await _roleManager.Roles.AnyAsync(r => r.Name == role))
                continue;
            await _roleManager.CreateAsync(new IdentityRole { Name = role });
        }
    }
}
