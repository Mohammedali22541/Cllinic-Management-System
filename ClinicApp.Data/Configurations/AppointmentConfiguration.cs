using ClinicApp.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicApp.Data.Configurations
{
    public class AppointmentConfiguration : BaseConfiguration<Appointment, int>
    {
        public override void Configure(EntityTypeBuilder<Appointment> builder)
        {
            base.Configure(builder);

            builder.Property(a => a.AppointmentDate)
                   .IsRequired();

            builder.Property(a => a.Status)
                   .IsRequired();

            builder.Property(a => a.Notes)
                   .HasMaxLength(1000);

            builder.HasOne(a => a.Doctor)
                   .WithMany(d => d.Appointments)
                   .HasForeignKey(a => a.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Patient)
                   .WithMany(p => p.Appointments)
                   .HasForeignKey(a => a.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.ApplicationUser)
                   .WithMany(u => u.Appointments)
                   .HasForeignKey(a => a.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}