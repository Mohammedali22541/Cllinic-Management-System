using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Data.Entites
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public Patient? PatientProfile { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}