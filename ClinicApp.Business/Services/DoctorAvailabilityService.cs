using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.AvailabilityDto;
using ClinicApp.Data.DatabaseContract;
using ClinicApp.Data.Entites;
using System.Linq.Expressions;

namespace ClinicApp.Business.Services
{
    public class DoctorAvailabilityService : IDoctorAvailabilityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorAvailabilityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DoctorAvailabilityListDto>> GetAllAsync()
        {
            var availabilities = await _unitOfWork
                .GetRepository<DoctorAvailability, int>()
                .GetAllAsync(
                    a => !a.IsDeleted,
                    new List<Expression<Func<DoctorAvailability, object>>>
                    {
                        a => a.Doctor
                    });

            return availabilities.Select(a => new DoctorAvailabilityListDto
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor.Name,
                DayOfWeek = a.DayOfWeek,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                SlotDurationInMinutes = a.SlotDurationInMinutes
            });
        }

        public async Task<UpdateDoctorAvailabilityDto?> GetForUpdateAsync(int id)
        {
            if (id <= 0)
                return null;

            var availability = await _unitOfWork
                .GetRepository<DoctorAvailability, int>()
                .GetByIdAsync(id);

            if (availability is null || availability.IsDeleted)
                return null;

            return new UpdateDoctorAvailabilityDto
            {
                Id = availability.Id,
                DoctorId = availability.DoctorId,
                DayOfWeek = availability.DayOfWeek,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                SlotDurationInMinutes = availability.SlotDurationInMinutes
            };
        }

        public async Task<bool> CreateAsync(CreateDoctorAvailabilityDto dto)
        {
            if (dto is null)
                return false;

            if (!IsValidAvailability(dto.DoctorId, dto.StartTime, dto.EndTime, dto.SlotDurationInMinutes))
                return false;

            var doctor = await _unitOfWork
                .GetRepository<Doctor, int>()
                .GetByIdAsync(dto.DoctorId);

            if (doctor is null || doctor.IsDeleted)
                return false;

            var hasConflict = (await _unitOfWork
                .GetRepository<DoctorAvailability, int>()
                .GetAllAsync(a =>
                    a.DoctorId == dto.DoctorId &&
                    a.DayOfWeek == dto.DayOfWeek &&
                    !a.IsDeleted &&
                    dto.StartTime < a.EndTime &&
                    dto.EndTime > a.StartTime))
                .Any();

            if (hasConflict)
                return false;

            var availability = new DoctorAvailability
            {
                DoctorId = dto.DoctorId,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                SlotDurationInMinutes = dto.SlotDurationInMinutes,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork
                .GetRepository<DoctorAvailability, int>()
                .AddAsync(availability);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(int id, UpdateDoctorAvailabilityDto dto)
        {
            if (id <= 0 || dto is null || id != dto.Id)
                return false;

            if (!IsValidAvailability(dto.DoctorId, dto.StartTime, dto.EndTime, dto.SlotDurationInMinutes))
                return false;

            var availability = await _unitOfWork
                .GetRepository<DoctorAvailability, int>()
                .GetByIdAsync(id);

            if (availability is null || availability.IsDeleted)
                return false;

            var doctor = await _unitOfWork
                .GetRepository<Doctor, int>()
                .GetByIdAsync(dto.DoctorId);

            if (doctor is null || doctor.IsDeleted)
                return false;

            var hasConflict = (await _unitOfWork
                .GetRepository<DoctorAvailability, int>()
                .GetAllAsync(a =>
                    a.Id != id &&
                    a.DoctorId == dto.DoctorId &&
                    a.DayOfWeek == dto.DayOfWeek &&
                    !a.IsDeleted &&
                    dto.StartTime < a.EndTime &&
                    dto.EndTime > a.StartTime))
                .Any();

            if (hasConflict)
                return false;

            availability.DoctorId = dto.DoctorId;
            availability.DayOfWeek = dto.DayOfWeek;
            availability.StartTime = dto.StartTime;
            availability.EndTime = dto.EndTime;
            availability.SlotDurationInMinutes = dto.SlotDurationInMinutes;
            availability.UpdatedAt = DateTime.UtcNow;

            _unitOfWork
                .GetRepository<DoctorAvailability, int>()
                .Update(availability);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                return false;

            var availability = await _unitOfWork
                .GetRepository<DoctorAvailability, int>()
                .GetByIdAsync(id);

            if (availability is null || availability.IsDeleted)
                return false;

            availability.IsDeleted = true;
            availability.UpdatedAt = DateTime.UtcNow;

            _unitOfWork
                .GetRepository<DoctorAvailability, int>()
                .Update(availability);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        private static bool IsValidAvailability(
            int doctorId,
            TimeOnly startTime,
            TimeOnly endTime,
            int slotDurationInMinutes)
        {
            if (doctorId <= 0)
                return false;

            if (startTime >= endTime)
                return false;

            if (slotDurationInMinutes <= 0)
                return false;

            var totalMinutes = (endTime.ToTimeSpan() - startTime.ToTimeSpan()).TotalMinutes;

            if (slotDurationInMinutes > totalMinutes)
                return false;

            return true;
        }
    }
}