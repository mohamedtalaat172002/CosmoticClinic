using Clinic.Domain.BaseEntity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic.Infrastructure.Persistence.Configurations
{
    public static class ImageBaseConfiguration
    {
        public static void ConfigureImageBase<T>(
            EntityTypeBuilder<T> builder)
            where T : ImageBase
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(2048);

            builder.Property(x => x.PublicId)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.DisplayOrder)
                .IsRequired();

            builder.Property(x => x.IsPrimary)
                .IsRequired();

            builder.HasIndex(x => x.PublicId)
                .IsUnique();
        }
    }
}
