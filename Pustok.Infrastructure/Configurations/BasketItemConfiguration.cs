using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Domain.Entities;

namespace Pustok.Infrastructure.Configurations;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.Property(x => x.Count).IsRequired();
        builder.HasIndex(x => new { x.ProductId, x.AppUserId }).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint("CK_BasketItem_Count_Not_Negative", "[Count]>0"));
        builder.HasOne(b=>b.AppUser).WithMany(u=>u.BasketItems).HasForeignKey(b => b.AppUserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(b=>b.Product).WithMany(u=>u.BasketItems).HasForeignKey(b => b.ProductId).OnDelete(DeleteBehavior.Restrict);
    }
}
