using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Domain.Entities;

namespace Pustok.Infrastructure.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(x=>x.IconUrl).IsRequired(false).HasMaxLength(256);
        builder.Property(x=>x.Description).IsRequired().HasMaxLength(256);
        builder.Property(x=>x.Title).IsRequired().HasMaxLength(256);
    }
}
