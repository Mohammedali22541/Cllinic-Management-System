using ClinicApp.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Data.Configurations
{
    public class DoctorAvailabilityConfiguration : BaseConfiguration<DoctorAvailability, int>
    {
        public override void Configure(EntityTypeBuilder<DoctorAvailability> builder)
        {
            base.Configure(builder);

            builder.Property(a => a.DayOfWeek)
                .IsRequired();

            builder.Property(a => a.StartTime)
                .IsRequired();

            builder.Property(a => a.EndTime)
                .IsRequired();

            builder.Property(a => a.SlotDurationInMinutes)
                .IsRequired();

            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Availabilities)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}