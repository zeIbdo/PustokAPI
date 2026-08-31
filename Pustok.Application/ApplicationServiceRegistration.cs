using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pustok.Application.AppSettingModels;
using Pustok.Application.Services.Abstractions;
using Pustok.Application.Services.Implementations;
using Pustok.Application.Validators.ProductValidators;
using System.Reflection;

namespace Pustok.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => { },Assembly.GetExecutingAssembly());
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IServiceEntityService, ServiceEntityService>();
        services.AddScoped<ISliderService, SliderService>();
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITagService, TagService>();
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IFileService, CloudinaryService>();
        services.AddValidatorsFromAssemblyContaining(typeof(ProductCreateDtoValidator));
        return services;
    }
}
