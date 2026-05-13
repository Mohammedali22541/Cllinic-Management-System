using ClinicApp.Data.Entites.Enums;

namespace ClinicApp.Business.Dtos.Appointments
{
    public class GetAllAppointmentListDto
    {
        public int Id { get; set; }

        public string DoctorName { get; set; } = null!;

        public string PatientName { get; set; } = null!;

        public string UserFullName { get; set; } = null!;

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatus Status { get; set; }
    }
}