using Clinic.Domain.Entities.treatment;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Configurations
{
    internal class TreatmentConfiguration : IEntityTypeConfiguration<Treatment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Treatment> builder)
        {
            builder.ToTable("Treatments");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(t => t.ImageUrl)
                .HasMaxLength(500);


            builder.HasMany(t => t.Articles)
                .WithOne(a => a.Treatment)
                .HasForeignKey(a => a.TreatmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}