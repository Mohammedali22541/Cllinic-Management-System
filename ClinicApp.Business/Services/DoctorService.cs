using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.Doctors;
using ClinicApp.Data.DatabaseContract;
using ClinicApp.Data.Entites;
using System.Linq.Expressions;

namespace ClinicApp.Business.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AllDoctorListDto>> GetAllAsync()
        {
            var doctors = await _unitOfWork
                .GetRepository<Doctor, int>()
                .GetAllAsync(d => !d.IsDeleted);

            return doctors.Select(d => new AllDoctorListDto
            {
                Id = d.Id,
                Name = d.Name,
                Phone = d.Phone,
                Specialization = d.Specialization.ToString()
            });
        }

        public async Task<DoctorDetailsDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var doctor = (await _unitOfWork
                .GetRepository<Doctor, int>()
                .GetAllAsync(
                    d => d.Id == id && !d.IsDeleted,
                    new List<Expression<Func<Doctor, object>>>
                    {
                        d => d.Appointments
                    }))
                .FirstOrDefault();

            if (doctor is null)
                return null;

            return new DoctorDetailsDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Phone = doctor.Phone,
                Specialization = doctor.Specialization.ToString(),
                AppointmentsCount = doctor.Appointments.Count(a => !a.IsDeleted)
            };
        }

        public async Task<bool> CreateAsync(CreateDoctorDto dto)
        {
            if (dto is null)
                return false;

            var doctor = new Doctor
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Specialization = dto.Specialization,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork
                .GetRepository<Doctor, int>()
                .AddAsync(doctor);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(int doctorId, UpdateDoctorDto dto)
        {
            if (doctorId <= 0 || dto is null)
                return false;

            var doctor = await _unitOfWork
                .GetRepository<Doctor, int>()
                .GetByIdAsync(doctorId);

            if (doctor is null || doctor.IsDeleted)
                return false;

            doctor.Name = dto.Name;
            doctor.Phone = dto.Phone;
            doctor.Specialization = dto.Specialization;
            doctor.UpdatedAt = DateTime.UtcNow;

            _unitOfWork
                .GetRepository<Doctor, int>()
                .Update(doctor);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                return false;

            var doctor = await _unitOfWork
                .GetRepository<Doctor, int>()
                .GetByIdAsync(id);

            if (doctor is null || doctor.IsDeleted)
                return false;

            var hasAppointments = await _unitOfWork
                .GetRepository<Appointment, int>()
                .GetAllAsync(a => a.DoctorId == id && a.AppointmentDate > DateTime.UtcNow && !a.IsDeleted);

            if (hasAppointments.Any())
            {
                return false;
            }
                doctor.IsDeleted = true;
            doctor.UpdatedAt = DateTime.UtcNow;

            _unitOfWork
                .GetRepository<Doctor, int>()
                .Update(doctor);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}