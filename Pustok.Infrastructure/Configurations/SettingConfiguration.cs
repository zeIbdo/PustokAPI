using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Domain.Entities;

namespace Pustok.Infrastructure.Configurations;

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.Property(x=>x.Key).IsRequired().HasMaxLength(256);
        builder.Property(x=>x.Value).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.Key).IsUnique();
    }
}
