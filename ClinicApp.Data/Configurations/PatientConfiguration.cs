using ClinicApp.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicApp.Data.Configurations
{
    public class PatientConfiguration : BaseConfiguration<Patient, int>
    {
        public override void Configure(EntityTypeBuilder<Patient> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(p => p.Age)
                   .IsRequired();

            builder.HasOne(p => p.ApplicationUser)
                   .WithOne(u => u.PatientProfile)
                   .HasForeignKey<Patient>(p => p.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}