using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.Doctors;
using ClinicApp.Data.Entites.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorsController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetAllAsync();
            return View(doctors);
        }

        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateDoctorDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var success = await _doctorService.CreateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to create doctor.");
                return View(dto);
            }

            TempData["Success"] = "Doctor created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            var updateDto = new UpdateDoctorDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialization = Enum.Parse<Specialization>(doctor.Specialization),
                Phone = doctor.Phone,
            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateDoctorDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(dto);

            var success = await _doctorService.UpdateAsync(id, dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to update doctor.");
                return View(dto);
            }

            TempData["Success"] = "Doctor updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _doctorService.DeleteAsync(id);

            if (!success)
            {
                TempData["Error"] = "Failed to delete doctor.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Doctor deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}