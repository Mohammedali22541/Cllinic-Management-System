using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Business.Dtos.Authentication
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}