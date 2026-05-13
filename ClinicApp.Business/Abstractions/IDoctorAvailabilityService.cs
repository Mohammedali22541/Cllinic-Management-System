using ClinicApp.Business.Dtos.AvailabilityDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Business.Abstractions
{
    public interface IDoctorAvailabilityService
    {
        Task<IEnumerable<DoctorAvailabilityListDto>> GetAllAsync();
        Task<UpdateDoctorAvailabilityDto?> GetForUpdateAsync(int id);
        Task<bool> CreateAsync(CreateDoctorAvailabilityDto dto);
        Task<bool> UpdateAsync(int id, UpdateDoctorAvailabilityDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
