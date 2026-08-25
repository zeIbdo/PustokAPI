using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.DataInitializers;
using Pustok.Infrastructure.Interceptors;
using System.Reflection;
using System.Reflection.Emit;

namespace Pustok.Infrastructure.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    private readonly AuditInterceptor _interceptor;
    public bool BypassAuditableInterceptor { get; set; } = false;
    public AppDbContext(DbContextOptions options, AuditInterceptor interceptor) : base(options)
    {
        _interceptor = interceptor;
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Tag> Tags { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.AddSettings();
        base.OnModelCreating(builder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_interceptor);
        base.OnConfiguring(optionsBuilder);
    }
}
