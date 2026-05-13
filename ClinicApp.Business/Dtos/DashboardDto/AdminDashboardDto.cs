namespace ClinicApp.Business.Dtos.DashboardDto
{
    public class AdminDashboardDto
    {
        public int DoctorsCount { get; set; }

        public int PatientsCount { get; set; }

        public int AppointmentsCount { get; set; }

        public int PendingAppointmentsCount { get; set; }
    }
}