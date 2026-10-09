using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic.Infrastructure.Persistence.Configurations
{
    public class ArticleImageConfiguration
        : IEntityTypeConfiguration<ArticleImage>
    {
        public void Configure(
            EntityTypeBuilder<ArticleImage> builder)
        {
            ImageBaseConfiguration.ConfigureImageBase(builder);

            builder.ToTable("ArticleImages");

            builder.Property(x => x.ArticleId)
                .IsRequired();

            builder.HasOne(x => x.Article)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.ArticleId,
                x.DisplayOrder
            });

            builder.HasIndex(x => x.ArticleId)
                .HasFilter("[IsPrimary] = 1")
                .IsUnique();
        }
    }
}
