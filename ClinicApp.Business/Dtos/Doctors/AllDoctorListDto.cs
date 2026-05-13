namespace ClinicApp.Business.Dtos.Doctors
{
    public class AllDoctorListDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Specialization { get; set; } = null!;
    }
}