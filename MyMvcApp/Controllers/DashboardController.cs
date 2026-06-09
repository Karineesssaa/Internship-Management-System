using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;

namespace MyMvcApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyProgress()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var myTasks = _context.TaskItems
                .Where(t => t.AssignedToUserId == userId);

            var myGrades = _context.Grades
                .Where(g => g.StudentId == userId);

            ViewBag.TotalTasks = await myTasks.CountAsync();
            ViewBag.NewTasks = await myTasks.CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.New);
            ViewBag.InProgressTasks = await myTasks.CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.InProgress);
            ViewBag.SubmittedTasks = await myTasks.CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.Submitted);
            ViewBag.ApprovedTasks = await myTasks.CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.Approved);
            ViewBag.RejectedTasks = await myTasks.CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.Rejected);
            ViewBag.NeedChangesTasks = await myTasks.CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.NeedChanges);

            ViewBag.AverageGrade = await myGrades.AnyAsync()
                ? Math.Round(await myGrades.AverageAsync(g => g.Value), 2)
                : 0;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminStatistics()
        {
            ViewBag.TotalUsers = await _context.Users.CountAsync();

            ViewBag.TotalStudents = await _context.UserRoles
                .Join(_context.Roles,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new { userRole, role })
                .CountAsync(x => x.role.Name == "Student");

            ViewBag.TotalMentors = await _context.UserRoles
                .Join(_context.Roles,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new { userRole, role })
                .CountAsync(x => x.role.Name == "Mentor");

            ViewBag.TotalTasks = await _context.TaskItems.CountAsync();

            ViewBag.NewTasks = await _context.TaskItems
                .CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.New);

            ViewBag.InProgressTasks = await _context.TaskItems
                .CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.InProgress);

            ViewBag.SubmittedTasks = await _context.TaskItems
                .CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.Submitted);

            ViewBag.ApprovedTasks = await _context.TaskItems
                .CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.Approved);

            ViewBag.RejectedTasks = await _context.TaskItems
                .CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.Rejected);

            ViewBag.NeedChangesTasks = await _context.TaskItems
                .CountAsync(t => t.Status == MyMvcApp.Models.TaskStatus.NeedChanges);

            ViewBag.TotalGroups = await _context.InternshipGroups.CountAsync();
            ViewBag.TotalSubmissions = await _context.Submissions.CountAsync();
            ViewBag.TotalGrades = await _context.Grades.CountAsync();

            ViewBag.AverageGrade = await _context.Grades.AnyAsync()
                ? Math.Round(await _context.Grades.AverageAsync(g => g.Value), 2)
                : 0;

            return View();
        }
    }
}