using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic.Infrastructure.Persistence.Configurations
{
    public class ClinicSettingImageConfiguration
        : IEntityTypeConfiguration<ClinicSettingImage>
    {
        public void Configure(
            EntityTypeBuilder<ClinicSettingImage> builder)
        {
            ImageBaseConfiguration.ConfigureImageBase(builder);

            builder.ToTable("ClinicSettingImages");
            //builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.ClinicSettingId)
                .IsRequired();

            builder.HasOne(x => x.ClinicSetting)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ClinicSettingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.ClinicSettingId,
                x.DisplayOrder
            });

            builder.HasIndex(x => x.ClinicSettingId)
                .HasFilter("[IsPrimary] = 1")
                .IsUnique();
        }
    }
}
