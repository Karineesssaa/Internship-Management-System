using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Admin");
            }

            if (User.IsInRole("Mentor"))
            {
                return RedirectToAction("Mentor");
            }

            if (User.IsInRole("Student"))
            {
                return RedirectToAction("Student");
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Mentor")]
        public IActionResult Mentor()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public IActionResult Student()
        {
            return View();
        }
    }
}