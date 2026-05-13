using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Business.Dtos.Appointments
{
    public class CreateAppointmentDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}