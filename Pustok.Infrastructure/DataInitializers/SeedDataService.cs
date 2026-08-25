using Microsoft.EntityFrameworkCore;
using Pustok.Domain.Entities;

namespace Pustok.Infrastructure.DataInitializers;

public static class SeedDataService
{
    public static void AddSettings(this ModelBuilder builder)
    {
        var settings = new List<Setting>
        {
            new Setting{Id =1,Key="SupportNumber",Value = "+1234567"},
            new Setting{Id =2,Key="Address",Value="ExampleAddress"},
            new Setting{Id =3,Key="Phone",Value="+2121212"},
            new Setting{Id =4,Key="Email",Value="example@gmail.com"}
        };
        builder.Entity<Setting>().HasData(settings);
    }
}
