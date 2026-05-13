using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.AvailabilityDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorAvailabilitiesController : Controller
    {
        private readonly IDoctorAvailabilityService _availabilityService;
        private readonly IDoctorService _doctorService;

        public DoctorAvailabilitiesController(
            IDoctorAvailabilityService availabilityService,
            IDoctorService doctorService)
        {
            _availabilityService = availabilityService;
            _doctorService = doctorService;
        }

        public async Task<IActionResult> Index()
        {
            var availabilities = await _availabilityService.GetAllAsync();
            return View(availabilities);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDoctorsAsync();
            return View(new CreateDoctorAvailabilityDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDoctorAvailabilityDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDoctorsAsync();
                return View(dto);
            }

            var success = await _availabilityService.CreateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to create availability. Please check the doctor, time range, duration, and overlapping slots.");
                await PopulateDoctorsAsync();
                return View(dto);
            }

            TempData["Success"] = "Doctor availability created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var availability = await _availabilityService.GetForUpdateAsync(id);

            if (availability is null)
                return NotFound();

            await PopulateDoctorsAsync();
            return View(availability);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateDoctorAvailabilityDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateDoctorsAsync();
                return View(dto);
            }

            var success = await _availabilityService.UpdateAsync(id, dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to update availability. Please check the doctor, time range, duration, and overlapping slots.");
                await PopulateDoctorsAsync();
                return View(dto);
            }

            TempData["Success"] = "Doctor availability updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var availability = (await _availabilityService.GetAllAsync())
                .FirstOrDefault(a => a.Id == id);

            if (availability is null)
                return NotFound();

            return View(availability);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _availabilityService.DeleteAsync(id);

            if (!success)
            {
                TempData["Error"] = "Failed to delete doctor availability.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Doctor availability deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDoctorsAsync()
        {
            var doctors = await _doctorService.GetAllAsync();
            ViewBag.Doctors = new SelectList(doctors, "Id", "Name");
        }
    }
}
