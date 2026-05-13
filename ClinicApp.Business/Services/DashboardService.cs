using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.DashboardDto;
using ClinicApp.Data.DatabaseContract;
using ClinicApp.Data.Entites;
using ClinicApp.Data.Entites.Enums;

namespace ClinicApp.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AdminDashboardDto> GetDashboardDataAsync()
        {
            var doctors = await _unitOfWork
                .GetRepository<Doctor, int>()
                .GetAllAsync(d => !d.IsDeleted);

            var patients = await _unitOfWork
                .GetRepository<Patient, int>()
                .GetAllAsync(p => !p.IsDeleted);

            var appointments = await _unitOfWork
                .GetRepository<Appointment, int>()
                .GetAllAsync(a => !a.IsDeleted);

            return new AdminDashboardDto
            {
                DoctorsCount = doctors.Count(),
                PatientsCount = patients.Count(),
                AppointmentsCount = appointments.Count(),
                PendingAppointmentsCount =
                    appointments.Count(a => a.Status == AppointmentStatus.Pending)
            };
        }
    }
}