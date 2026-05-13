using ClinicApp.Business.Dtos.Doctors;

namespace ClinicApp.Business.Abstractions
{
    public interface IDoctorService
    {
        Task<IEnumerable<AllDoctorListDto>> GetAllAsync();

        Task<DoctorDetailsDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateDoctorDto dto);

        Task<bool> UpdateAsync(int doctorId, UpdateDoctorDto dto);

        Task<bool> DeleteAsync(int id);
    }
}