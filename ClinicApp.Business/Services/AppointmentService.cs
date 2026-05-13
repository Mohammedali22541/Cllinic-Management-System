using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.Appointments;
using ClinicApp.Data.DatabaseContract;
using ClinicApp.Data.Entites;
using ClinicApp.Data.Entites.Enums;
using System.Linq.Expressions;

namespace ClinicApp.Business.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(CreateAppointmentDto dto, string userId)
        {
            if (dto is null || string.IsNullOrWhiteSpace(userId))
                return false;

            if (dto.AppointmentDate == DateTime.MinValue)
                dto.AppointmentDate = DateTime.Now;

            if (dto.AppointmentDate < DateTime.Now)
                return false;

            var doctor = await _unitOfWork
                .GetRepository<Doctor, int>()
                .GetByIdAsync(dto.DoctorId);

            if (doctor is null || doctor.IsDeleted)
                return false;

            Patient? patient;

            if (dto.PatientId > 0)
            {
                patient = await _unitOfWork
                    .GetRepository<Patient, int>()
                    .GetByIdAsync(dto.PatientId);
            }
            else
            {
                patient = (await _unitOfWork
                    .GetRepository<Patient, int>()
                    .GetAllAsync(p => p.ApplicationUserId == userId && !p.IsDeleted))
                    .FirstOrDefault();
            }

            if (patient is null || patient.IsDeleted)
                return false;


            var appointmentDay = dto.AppointmentDate.DayOfWeek;
var appointmentTime = TimeOnly.FromDateTime(dto.AppointmentDate);

var availability = (await _unitOfWork
    .GetRepository<DoctorAvailability, int>()
    .GetAllAsync(a =>
        a.DoctorId == dto.DoctorId &&
        a.DayOfWeek == appointmentDay &&
        !a.IsDeleted &&
        appointmentTime >= a.StartTime &&
        appointmentTime < a.EndTime))
    .FirstOrDefault();

if (availability is null)
    return false;

var minutesFromStart =
    (appointmentTime.ToTimeSpan() - availability.StartTime.ToTimeSpan()).TotalMinutes;

if (minutesFromStart % availability.SlotDurationInMinutes != 0)
    return false;
            var isBooked = (await _unitOfWork
            .GetRepository<Appointment, int>()
            .GetAllAsync(a =>
                a.DoctorId == dto.DoctorId &&
                a.AppointmentDate == dto.AppointmentDate &&
                !a.IsDeleted &&
                a.Status != AppointmentStatus.Cancelled))
            .Any();

            if (isBooked)
                return false;

            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = patient.Id,
                AppointmentDate = dto.AppointmentDate,
                Notes = dto.Notes,
                Status = AppointmentStatus.Pending,
                ApplicationUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork
                .GetRepository<Appointment, int>()
                .AddAsync(appointment);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                return false;

            var appointment = await _unitOfWork
                .GetRepository<Appointment, int>()
                .GetByIdAsync(id);

            if (appointment is null || appointment.IsDeleted)
                return false;

            appointment.IsDeleted = true;
            appointment.UpdatedAt = DateTime.UtcNow;

            _unitOfWork
                .GetRepository<Appointment, int>()
                .Update(appointment);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<GetAllAppointmentListDto>> GetAllAsync()
        {
            var appointments = await _unitOfWork
                .GetRepository<Appointment, int>()
                .GetAllAsync(
                    a => !a.IsDeleted,
                    new List<Expression<Func<Appointment, object>>>
                    {
                        a => a.Doctor,
                        a => a.Patient,
                        a => a.ApplicationUser
                    });

            return appointments.Select(a => new GetAllAppointmentListDto
            {
                Id = a.Id,
                DoctorName = a.Doctor.Name,
                PatientName = a.Patient.Name,
                UserFullName = a.ApplicationUser.FullName,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status
            });
        }

        public async Task<AppointmentDetailsDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var appointment = (await _unitOfWork
                .GetRepository<Appointment, int>()
                .GetAllAsync(
                    a => a.Id == id && !a.IsDeleted,
                    new List<Expression<Func<Appointment, object>>>
                    {
                        a => a.Doctor,
                        a => a.Patient,
                        a => a.ApplicationUser
                    }))
                .FirstOrDefault();

            if (appointment is null)
                return null;

            return new AppointmentDetailsDto
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.Name,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.Name,
                UserId = appointment.ApplicationUserId,
                UserFullName = appointment.ApplicationUser.FullName,
                AppointmentDate = appointment.AppointmentDate,
                Notes = appointment.Notes,
                Status = appointment.Status
            };
        }

        public async Task<IEnumerable<GetAllAppointmentListDto>> GetUserAppointmentsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return Enumerable.Empty<GetAllAppointmentListDto>();

            var appointments = await _unitOfWork
                .GetRepository<Appointment, int>()
                .GetAllAsync(
                    a => a.ApplicationUserId == userId && !a.IsDeleted,
                    new List<Expression<Func<Appointment, object>>>
                    {
                        a => a.Doctor,
                        a => a.Patient,
                        a => a.ApplicationUser
                    });

            return appointments.Select(a => new GetAllAppointmentListDto
            {
                Id = a.Id,
                DoctorName = a.Doctor.Name,
                PatientName = a.Patient.Name,
                UserFullName = a.ApplicationUser.FullName,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status
            });
        }

        public async Task<bool> UpdateStatusAsync(int appointmentId, AppointmentStatus status)
        {
            if (appointmentId <= 0)
                return false;

            var appointment = await _unitOfWork
                .GetRepository<Appointment, int>()
                .GetByIdAsync(appointmentId);

            if (appointment is null || appointment.IsDeleted)
                return false;

            if (!CanChangeStatus(appointment.Status, status))
                return false;

            if (appointment.Status == status)
                return true;

            appointment.Status = status;
            appointment.UpdatedAt = DateTime.UtcNow;

            _unitOfWork
                .GetRepository<Appointment, int>()
                .Update(appointment);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelUserPendingAsync(int appointmentId, string userId)
        {
            if (appointmentId <= 0 || string.IsNullOrWhiteSpace(userId))
                return false;

            var appointment = (await _unitOfWork
                .GetRepository<Appointment, int>()
                .GetAllAsync(a => a.Id == appointmentId && a.ApplicationUserId == userId && !a.IsDeleted))
                .FirstOrDefault();

            if (appointment is null || appointment.Status != AppointmentStatus.Pending)
                return false;

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTime.UtcNow;

            _unitOfWork
                .GetRepository<Appointment, int>()
                .Update(appointment);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        private static bool CanChangeStatus(AppointmentStatus currentStatus, AppointmentStatus newStatus)
        {
            if (currentStatus == newStatus)
                return true;

            return currentStatus == AppointmentStatus.Pending &&
                   (newStatus == AppointmentStatus.Completed ||
                    newStatus == AppointmentStatus.Cancelled);
        }
    }
}
