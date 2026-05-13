using ClinicApp.Business.Dtos.DashboardDto;

namespace ClinicApp.Business.Abstractions
{
    public interface IDashboardService
    {
        Task<AdminDashboardDto> GetDashboardDataAsync();
    }
}