using ClinicApp.Data.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicApp.Data.Configurations
{
    public class DoctorConfiguration : BaseConfiguration<Doctor, int>
    {
        public override void Configure(EntityTypeBuilder<Doctor> builder)
        {
            base.Configure(builder);
            builder.Property(d => d.Name)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(d => d.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(d => d.Specialization)
                   .IsRequired();
        }
    }
}