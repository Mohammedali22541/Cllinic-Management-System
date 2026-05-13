using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.Patients;
using ClinicApp.Data.DatabaseContract;
using ClinicApp.Data.Entites;
using System.Linq.Expressions;

namespace ClinicApp.Business.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(CreatePatientDto dto)
        {
            if (dto is null)
                return false;

            var patient = new Patient
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Age = dto.Age,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork
                .GetRepository<Patient, int>()
                .AddAsync(patient);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                return false;

            var patient = await _unitOfWork
                .GetRepository<Patient, int>()
                .GetByIdAsync(id);

            if (patient is null || patient.IsDeleted)
                return false;

            var hasFutureAppointments = (await _unitOfWork
                .GetRepository<Appointment, int>()
                .GetAllAsync(a => a.PatientId == id && a.AppointmentDate > DateTime.UtcNow))
                .Any();

            if (hasFutureAppointments)
            {

                return false;
            }


                patient.IsDeleted = true;
            patient.UpdatedAt = DateTime.UtcNow;

            _unitOfWork
                .GetRepository<Patient, int>()
                .Update(patient);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<GetAllPatientListDto>> GetAllAsync()
        {
            var patients = await _unitOfWork
                .GetRepository<Patient, int>()
                .GetAllAsync(p => !p.IsDeleted);

            return patients.Select(p => new GetAllPatientListDto
            {
                Id = p.Id,
                Name = p.Name,
                Phone = p.Phone,
                Age = p.Age
            });
        }

        public async Task<PatientDetailsDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var patient = (await _unitOfWork
                .GetRepository<Patient, int>()
                .GetAllAsync(
                    p => p.Id == id && !p.IsDeleted,
                    new List<Expression<Func<Patient, object>>>
                    {
                        p => p.Appointments
                    }))
                .FirstOrDefault();

            if (patient is null)
                return null;

            return new PatientDetailsDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Phone = patient.Phone,
                Age = patient.Age,
                AppointmentsCount = patient.Appointments.Count
            };
        }

        public async Task<bool> UpdateAsync(int patientId, UpdatePatientDto dto)
        {
            if (patientId <= 0 || dto is null)
                return false;

            var patient = await _unitOfWork
                .GetRepository<Patient, int>()
                .GetByIdAsync(patientId);

            if (patient is null || patient.IsDeleted)
                return false;

            patient.Name = dto.Name;
            patient.Phone = dto.Phone;
            patient.Age = dto.Age;
            patient.UpdatedAt = DateTime.UtcNow;

            _unitOfWork
                .GetRepository<Patient, int>()
                .Update(patient);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<PatientProfileDto?> GetMyPatientProfileAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            var patient = (await _unitOfWork
                .GetRepository<Patient, int>()
                .GetAllAsync(p => p.ApplicationUserId == userId && !p.IsDeleted))
                .FirstOrDefault();

            if (patient is null)
                return null;

            return new PatientProfileDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Phone = patient.Phone,
                Age = patient.Age
            };
        }

        public async Task<bool> CreateOrUpdateMyPatientProfileAsync(string userId, PatientProfileDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId) || dto is null)
                return false;

            var patient = (await _unitOfWork
                .GetRepository<Patient, int>()
                .GetAllAsync(p => p.ApplicationUserId == userId && !p.IsDeleted))
                .FirstOrDefault();

            if (patient is null)
            {
                patient = new Patient
                {
                    Name = dto.Name,
                    Phone = dto.Phone,
                    Age = dto.Age,
                    ApplicationUserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork
                    .GetRepository<Patient, int>()
                    .AddAsync(patient);
            }
            else
            {
                patient.Name = dto.Name;
                patient.Phone = dto.Phone;
                patient.Age = dto.Age;
                patient.UpdatedAt = DateTime.UtcNow;

                _unitOfWork
                    .GetRepository<Patient, int>()
                    .Update(patient);
            }

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}