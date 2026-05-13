using ClinicApp.Data.Entites.Common;
using ClinicApp.Data.Entites.Enums;

namespace ClinicApp.Data.Entites
{
    public class Doctor : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Specialization Specialization { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<DoctorAvailability> Availabilities { get; set; } = new List<DoctorAvailability>();
    }
}