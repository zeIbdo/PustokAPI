using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pustok.Domain.Entities;

namespace Pustok.Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(x => x.Token).IsRequired().HasMaxLength(500);
        builder.Property(x => x.AppUserId).IsRequired();

        builder.HasOne(x => x.AppUser)
        .WithMany()
        .HasForeignKey(x => x.AppUserId)
        .OnDelete(DeleteBehavior.NoAction);
    }
}