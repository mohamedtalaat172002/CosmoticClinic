using Clinic.Domain.Entities.clinicSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic.Infrastructure.Persistence.Configurations
{

    public class ClinicSettingsConfiguration : IEntityTypeConfiguration<ClinicSettings>
    {
        private static readonly Guid ClinicSettingsId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        public void Configure(EntityTypeBuilder<ClinicSettings> builder)
        {
            builder.ToTable("ClinicSettings", table =>
            {
                table.HasCheckConstraint(
                    "CK_ClinicSettings_SingleRow",
                    $"[Id] = '{ClinicSettingsId}'");
            });

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ClinicName)
                .HasMaxLength(150);

            builder.Property(x => x.DoctorName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Specialization)
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.WhatsAppNumber)
                .HasMaxLength(30);

            builder.Property(x => x.Email)
                .HasMaxLength(255);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.LogoUrl)
                .HasMaxLength(500);

            builder.Property(x => x.HeroImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.FacebookUrl)
                .HasMaxLength(500);

            builder.Property(x => x.InstagramUrl)
                .HasMaxLength(500);

            builder.HasData(new ClinicSettings
            {
                Id = ClinicSettingsId,
                ClinicName = "Clinic Name",
                DoctorName = "Doctor Name",
                Address = "Clinic Address"
            });
        }
    }
}
