using ClinicApp.Data.Entites.Common;
using ClinicApp.Data.Entites.Enums;

namespace ClinicApp.Data.Entites
{
    public class Appointment : BaseEntity<int>
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public DateTime AppointmentDate { get; set; }

        public string? Notes { get; set; }

        public AppointmentStatus Status { get; set; }

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}