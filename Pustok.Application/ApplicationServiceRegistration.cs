using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Pustok.Application.Services.Abstractions;
using Pustok.Application.Services.Implementations;
using Pustok.Application.Validators.ProductValidators;
using System.Reflection;

namespace Pustok.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { },Assembly.GetExecutingAssembly());
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IServiceEntityService, ServiceEntityService>();
        services.AddScoped<ISliderService, SliderService>();
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<ITagService, TagService>();
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IFileService, CloudinaryService>();
        services.AddValidatorsFromAssemblyContaining(typeof(ProductCreateDtoValidator));
        return services;
    }
}
