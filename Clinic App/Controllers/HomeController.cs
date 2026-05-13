using Microsoft.AspNetCore.Mvc;

namespace Clinic_App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // If user is not authenticated, redirect to login
            if (User?.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Account");
            }

            // If user is Admin, redirect to Dashboard
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            // For normal users, redirect to Appointments
            return RedirectToAction("Index", "Appointments");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}