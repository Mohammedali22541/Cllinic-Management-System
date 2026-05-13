using ClinicApp.Data.Entites.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Business.Dtos.Appointments
{
    public class UpdateAppointmentStatusDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }
    }
}