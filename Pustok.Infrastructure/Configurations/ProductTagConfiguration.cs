using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Domain.Entities;

namespace Pustok.Infrastructure.Configurations;

public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.HasOne(x => x.Product).WithMany(p => p.ProductTags).HasForeignKey(s=>s.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Tag).WithMany(p => p.ProductTags).HasForeignKey(s=>s.TagId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.ProductId, x.TagId }).IsUnique();
        builder.HasQueryFilter(v => !v.Product.IsDeleted);
    }
}
