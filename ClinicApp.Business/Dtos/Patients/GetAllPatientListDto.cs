namespace ClinicApp.Business.Dtos.Patients
{
    public class GetAllPatientListDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public int Age { get; set; }
    }
}