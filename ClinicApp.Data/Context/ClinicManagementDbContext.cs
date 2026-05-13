using ClinicApp.Data.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ClinicApp.Data.Context
{
    public class ClinicManagementDbContext : IdentityDbContext<ApplicationUser>
    {
        public ClinicManagementDbContext(DbContextOptions<ClinicManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
    }
}