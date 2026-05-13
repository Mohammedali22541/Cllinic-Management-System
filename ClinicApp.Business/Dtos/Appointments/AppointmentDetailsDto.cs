using ClinicApp.Data.Entites.Enums;

namespace ClinicApp.Business.Dtos.Appointments
{
    public class AppointmentDetailsDto
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = null!;

        public int PatientId { get; set; }

        public string PatientName { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string UserFullName { get; set; } = null!;

        public DateTime AppointmentDate { get; set; }

        public string? Notes { get; set; }

        public AppointmentStatus Status { get; set; }
    }
}