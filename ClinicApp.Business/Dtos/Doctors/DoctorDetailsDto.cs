namespace ClinicApp.Business.Dtos.Doctors
{
    public class DoctorDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Specialization { get; set; } = null!;

        public int AppointmentsCount { get; set; }
    }
}