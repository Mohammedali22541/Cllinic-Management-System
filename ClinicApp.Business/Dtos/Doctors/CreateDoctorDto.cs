using ClinicApp.Data.Entites.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Business.Dtos.Doctors
{
    public class CreateDoctorDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        [Required]
        public Specialization Specialization { get; set; }
    }
}