using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Domain.Entities;

namespace Pustok.Infrastructure.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(256);
        builder.Property(x => x.IsMain).HasDefaultValue(false);
        builder.HasIndex(x => new {x.ProductId,x.IsMain}).IsUnique().HasFilter("[IsMain]=1");
        builder.HasQueryFilter(v => !v.Product.IsDeleted);
    }
}
