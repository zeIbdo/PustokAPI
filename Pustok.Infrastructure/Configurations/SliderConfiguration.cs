using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Domain.Entities;

namespace Pustok.Infrastructure.Configurations;

public class SliderConfiguration : IEntityTypeConfiguration<Slider>
{
    public void Configure(EntityTypeBuilder<Slider> builder)
    {
        builder.Property(s => s.ImageUrl).IsRequired(false).HasMaxLength(256);
        builder.Property(s => s.Title).IsRequired().HasMaxLength(256);
        builder.Property(s => s.Description).IsRequired().HasMaxLength(256);
        builder.Property(s => s.Price).IsRequired().HasColumnType("decimal(8,2)");
        builder.ToTable(t => t.HasCheckConstraint("CK_Slider_Price_Not_Negative", "[Price]>0"));
    }
}