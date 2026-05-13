using ClinicApp.Data.Entites.Common;

namespace ClinicApp.Data.Entites
{
    public class Patient : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int Age { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}