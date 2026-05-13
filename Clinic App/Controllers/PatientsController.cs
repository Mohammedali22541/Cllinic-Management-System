using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PatientsController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetAllAsync();
            return View(patients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient is null)
                return NotFound();

            return View(patient);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePatientDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var success = await _patientService.CreateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to create patient.");
                return View(dto);
            }

            TempData["Success"] = "Patient created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient is null)
                return NotFound();

            var updateDto = new UpdatePatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Age = patient.Age,
                Phone = patient.Phone,
            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdatePatientDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(dto);

            var success = await _patientService.UpdateAsync(id, dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to update patient.");
                return View(dto);
            }

            TempData["Success"] = "Patient updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient is null)
                return NotFound();

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _patientService.DeleteAsync(id);

            if (!success)
            {
                TempData["Error"] = "Failed to delete patient.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Patient deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}