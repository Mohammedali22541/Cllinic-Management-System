using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Business.Dtos.Patients
{
    public class CreatePatientDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        [Range(1, 120)]
        public int Age { get; set; }
    }
}