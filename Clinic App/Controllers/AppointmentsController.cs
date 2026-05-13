using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.Appointments;
using ClinicApp.Data.Entites.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Clinic_App.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;

        public AppointmentsController(IAppointmentService appointmentService, IDoctorService doctorService, IPatientService patientService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var appointments = await _appointmentService.GetAllAsync();
                return View(appointments);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var userAppointments = await _appointmentService.GetUserAppointmentsAsync(userId);
            return View(userAppointments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);

            if (appointment is null)
                return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (appointment.UserId != userId)
                    return Forbid();
            }

            return View(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!User.IsInRole("Admin"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized();

                var patientProfile = await _patientService.GetMyPatientProfileAsync(userId);

                if (patientProfile is null)
                {
                    TempData["Error"] = "Please complete your patient profile before booking an appointment.";
                    return RedirectToAction("MyProfile", "Profile");
                }
            }

            await PopulateSelectionsAsync();
            return View(new CreateAppointmentDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectionsAsync();
                return View(dto);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            if (!User.IsInRole("Admin"))
            {
                var patientProfile = await _patientService.GetMyPatientProfileAsync(userId);

                if (patientProfile is null)
                {
                    TempData["Error"] = "Please complete your patient profile before booking an appointment.";
                    return RedirectToAction("MyProfile", "Profile");
                }

                dto.PatientId = 0;
            }

            var success = await _appointmentService.CreateAsync(dto, userId);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to create appointment.");
                await PopulateSelectionsAsync();
                return View(dto);
            }

            TempData["Success"] = "Appointment created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (User.IsInRole("Admin"))
            {
                var adminSuccess = await _appointmentService.UpdateStatusAsync(id, AppointmentStatus.Cancelled);
                TempData[adminSuccess ? "Success" : "Error"] = adminSuccess
                    ? "Appointment cancelled successfully."
                    : "Only pending appointments can be cancelled.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var success = await _appointmentService.CancelUserPendingAsync(id, userId);
            TempData[success ? "Success" : "Error"] = success
                ? "Appointment cancelled successfully."
                : "Only your pending appointments can be cancelled.";

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int appointmentId, AppointmentStatus status)
        {
            var success = await _appointmentService.UpdateStatusAsync(appointmentId, status);

            if (!success)
                TempData["Error"] = "Invalid appointment status transition.";
            else
                TempData["Success"] = "Appointment status updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);

            if (appointment is null)
                return NotFound();

            return View(appointment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _appointmentService.DeleteAsync(id);

            if (!success)
            {
                TempData["Error"] = "Failed to delete appointment.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Appointment deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateSelectionsAsync()
        {
            var doctors = await _doctorService.GetAllAsync();
            ViewBag.Doctors = new SelectList(doctors, "Id", "Name");

            if (User.IsInRole("Admin"))
            {
                var patients = await _patientService.GetAllAsync();
                ViewBag.Patients = new SelectList(patients, "Id", "Name");
            }
        }
    }
}
