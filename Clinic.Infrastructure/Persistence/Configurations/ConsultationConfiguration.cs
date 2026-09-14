using Clinic.Domain.Entities.consultation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic.Infrastructure.Persistence.Configurations
{

    public sealed class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
    {
        public void Configure(EntityTypeBuilder<Consultation> builder)
        {
            builder.ToTable("Consultations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);


            builder.Property(x => x.Message)
                .HasMaxLength(500);


            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasIndex(x => x.Status);


        }
    }
}
