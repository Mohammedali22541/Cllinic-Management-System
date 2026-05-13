using ClinicApp.Business.Dtos.Patients;

namespace ClinicApp.Business.Abstractions
{
    public interface IPatientService
    {
        Task<IEnumerable<GetAllPatientListDto>> GetAllAsync();

        Task<PatientDetailsDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreatePatientDto dto);

        Task<bool> UpdateAsync(int patientId, UpdatePatientDto dto);

        Task<bool> DeleteAsync(int id);

        Task<PatientProfileDto?> GetMyPatientProfileAsync(string userId);

        Task<bool> CreateOrUpdateMyPatientProfileAsync(string userId, PatientProfileDto dto);
    }
}