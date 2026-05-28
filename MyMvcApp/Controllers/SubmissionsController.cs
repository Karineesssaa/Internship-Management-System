using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    [Authorize]
    public class SubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubmissionsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Submissions
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> Index()
        {
            var submissions = _context.Submissions
                .Include(s => s.Student)
                .Include(s => s.TaskItem);

            return View(await submissions.ToListAsync());
        }

        // GET: Submissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submissions
                .Include(s => s.Student)
                .Include(s => s.TaskItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (submission == null)
            {
                return NotFound();
            }

            return View(submission);
        }

        // GET: Submissions/Create?taskItemId=5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(int taskItemId)
        {
            var userId = _userManager.GetUserId(User);

            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == taskItemId && t.AssignedToUserId == userId);

            if (task == null)
            {
                return Forbid();
            }

            var submission = new Submission
            {
                TaskItemId = task.Id,
                TaskItem = task
            };

            return View(submission);
        }

        // POST: Submissions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create([Bind("TaskItemId,GitHubLink,Comment,FilePath")] Submission submission)
        {
            var userId = _userManager.GetUserId(User);

            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == submission.TaskItemId && t.AssignedToUserId == userId);

            if (task == null)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                submission.StudentId = userId;
                submission.SubmittedAt = DateTime.Now;

                task.Status = MyMvcApp.Models.TaskStatus.Submitted;

                _context.Add(submission);
                await _context.SaveChangesAsync();

                return RedirectToAction("MyTasks", "TaskItems");
            }

            return View(submission);
        }

        // GET: Submissions/Edit/5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var submission = await _context.Submissions
                .Include(s => s.TaskItem)
                .FirstOrDefaultAsync(s => s.Id == id && s.StudentId == userId);

            if (submission == null)
            {
                return NotFound();
            }

            return View(submission);
        }

        // POST: Submissions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskItemId,GitHubLink,Comment,FilePath")] Submission submission)
        {
            if (id != submission.Id)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var existingSubmission = await _context.Submissions
                .FirstOrDefaultAsync(s => s.Id == id && s.StudentId == userId);

            if (existingSubmission == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                existingSubmission.GitHubLink = submission.GitHubLink;
                existingSubmission.Comment = submission.Comment;
                existingSubmission.FilePath = submission.FilePath;
                existingSubmission.SubmittedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return RedirectToAction("MyTasks", "TaskItems");
            }

            return View(submission);
        }

        // GET: Submissions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submissions
                .Include(s => s.Student)
                .Include(s => s.TaskItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (submission == null)
            {
                return NotFound();
            }

            return View(submission);
        }


        // GET: Submissions/Review/5
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> Review(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submissions
                .Include(s => s.Student)
                .Include(s => s.TaskItem)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (submission == null)
            {
                return NotFound();
            }

            return View(submission);
        }

        // POST: Submissions/Review/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> Review(int id, MyMvcApp.Models.TaskStatus status)
        {
            var submission = await _context.Submissions
                .Include(s => s.TaskItem)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (submission == null || submission.TaskItem == null)
            {
                return NotFound();
            }

            submission.TaskItem.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var submission = await _context.Submissions.FindAsync(id);

            if (submission != null)
            {
                _context.Submissions.Remove(submission);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SubmissionExists(int id)
        {
            return _context.Submissions.Any(e => e.Id == id);
        }
    }
}