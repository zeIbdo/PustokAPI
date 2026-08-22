using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Contexts;
using Pustok.Infrastructure.Interceptors;
using Pustok.Infrastructure.Repositories.Abstractions;
using Pustok.Infrastructure.Repositories.Abstractions.Generic;
using Pustok.Infrastructure.Repositories.Implementations;
using Pustok.Infrastructure.Repositories.Implementations.Generic;

namespace Pustok.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
            options.Lockout.AllowedForNewUsers = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 3;
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<DbContextInitializer>();
        services.AddScoped(typeof(IRepositoryAsync<>), typeof(Repository<>));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBasketItemRepository, BasketItemRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IProductTagRepository, ProductTagRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ISettingRepository, SettingRepository>();
        services.AddScoped<ISliderRepository, SliderRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<ITagRepository, TagRepository>();

        return services;
    }
}
