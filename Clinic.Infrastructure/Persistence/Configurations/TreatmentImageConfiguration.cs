using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic.Infrastructure.Persistence.Configurations
{
    public class TreatmentImageConfiguration
        : IEntityTypeConfiguration<TreatmentImage>
    {
        public void Configure(
            EntityTypeBuilder<TreatmentImage> builder)
        {
            ImageBaseConfiguration.ConfigureImageBase(builder);

            builder.ToTable("TreatmentImages");
            //builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.TreatmentId)
                .IsRequired();

            builder.HasOne(x => x.Treatment)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.TreatmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.TreatmentId,
                x.DisplayOrder
            });

            builder.HasIndex(x => x.TreatmentId)
                .HasFilter("[IsPrimary] = 1")
                .IsUnique();
        }
    }
}
