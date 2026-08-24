using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Domain.Entities;

namespace Pustok.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.ProductCode).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(1024);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(8,2)");
        builder.Property(x => x.Discount).IsRequired().HasColumnType("decimal(8,2)");
        builder.Property(x => x.RatingStar).IsRequired(false).HasColumnType("decimal(2,1)");
        builder.Property(x => x.Stock).IsRequired();
        builder.Property(x => x.ViewCount).HasDefaultValue(0);
        builder.HasIndex(x => x.ProductCode).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("CK_Product_Discount_Range", "[Discount]>=0 AND [Discount]<=100"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Product_RatingStar_Range", "[RatingStar]>0 AND [RatingStar]<=5"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Product_Price_Not_Negative", "[Price]>0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Product_Stock_Positive", "[Stock]>=0"));
        builder.HasOne(b => b.Category).WithMany(u => u.Products).HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
