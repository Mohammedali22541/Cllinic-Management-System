using ClinicApp.Business.Abstractions;
using ClinicApp.Business.Dtos.Patients;
using ClinicApp.Data.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clinic_App.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(IPatientService patientService, UserManager<ApplicationUser> userManager)
        {
            _patientService = patientService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var profile = await _patientService.GetMyPatientProfileAsync(userId);

            if (profile is not null)
                return View(profile);

            var user = await _userManager.GetUserAsync(User);

            return View(new PatientProfileDto
            {
                Name = user?.FullName ?? string.Empty,
                Phone = user?.PhoneNumber ?? string.Empty
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyProfile(PatientProfileDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var success = await _patientService.CreateOrUpdateMyPatientProfileAsync(userId, dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to save your patient profile.");
                return View(dto);
            }

            TempData["Success"] = "Patient profile saved successfully.";
            return RedirectToAction(nameof(MyProfile));
        }
    }
}