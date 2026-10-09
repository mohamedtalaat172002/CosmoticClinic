using Clinic.Domain.Entities.artical;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic.Infrastructure.Persistence.Configurations
{
    internal class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Articles");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Slug)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(a => a.Slug)
                .IsUnique();

            builder.Property(a => a.Summary)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(a => a.Content)
                .IsRequired()
                .HasMaxLength(3000);


            builder.Property(a => a.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.PublishedAt);

            builder.HasOne(a => a.Treatment)
     .WithMany(t => t.Articles)
     .HasForeignKey(a => a.TreatmentId)
     .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
