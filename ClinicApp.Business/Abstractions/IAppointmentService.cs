using ClinicApp.Business.Dtos.Appointments;
using ClinicApp.Data.Entites.Enums;

namespace ClinicApp.Business.Abstractions
{
    public interface IAppointmentService
    {
        Task<IEnumerable<GetAllAppointmentListDto>> GetAllAsync();

        Task<IEnumerable<GetAllAppointmentListDto>> GetUserAppointmentsAsync(string userId);

        Task<AppointmentDetailsDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateAppointmentDto dto, string userId);

        Task<bool> UpdateStatusAsync(int appointmentId, AppointmentStatus status);

        Task<bool> CancelUserPendingAsync(int appointmentId, string userId);

        Task<bool> DeleteAsync(int id);
    }
}