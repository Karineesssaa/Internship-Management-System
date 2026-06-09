using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyMvcApp.Controllers
{
    [Authorize]
    public class GradesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Grades
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> Index()
        {
            var grades = _context.Grades
                .Include(g => g.Student)
                .Include(g => g.TaskItem);

            return View(await grades.ToListAsync());
        }

        // GET: Grades/MyGrades
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyGrades()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var grades = await _context.Grades
                .Include(g => g.TaskItem)
                .Where(g => g.StudentId == userId)
                .ToListAsync();

            return View(grades);
        }

        // GET: Grades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.TaskItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // GET: Grades/Create?submissionId=5
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> Create(int submissionId)
        {
            var submission = await _context.Submissions
                .Include(s => s.Student)
                .Include(s => s.TaskItem)
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null || submission.TaskItem == null)
            {
                return NotFound();
            }

            var existingGrade = await _context.Grades
                .FirstOrDefaultAsync(g =>
                    g.TaskItemId == submission.TaskItemId &&
                    g.StudentId == submission.StudentId);

            if (existingGrade != null)
            {
                return RedirectToAction(nameof(Edit), new { id = existingGrade.Id });
            }

            var grade = new Grade
            {
                TaskItemId = submission.TaskItemId,
                TaskItem = submission.TaskItem,
                StudentId = submission.StudentId,
                Student = submission.Student
            };

            return View(grade);
        }

        // POST: Grades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> Create([Bind("TaskItemId,StudentId,Value,MentorComment")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                grade.CreatedAt = DateTime.Now;

                _context.Add(grade);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            grade.TaskItem = await _context.TaskItems.FindAsync(grade.TaskItemId);
            grade.Student = await _context.Users.FindAsync(grade.StudentId);

            return View(grade);
        }

        // GET: Grades/Edit/5
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.TaskItem)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grade == null)
            {
                return NotFound();
            }

            ViewData["TaskItemId"] = new SelectList(_context.TaskItems, "Id", "Title", grade.TaskItemId);
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Email", grade.StudentId);

            return View(grade);
        }

        // POST: Grades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskItemId,StudentId,Value,MentorComment")] Grade grade)
        {
            if (id != grade.Id)
            {
                return NotFound();
            }

            var existingGrade = await _context.Grades.FindAsync(id);

            if (existingGrade == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                existingGrade.TaskItemId = grade.TaskItemId;
                existingGrade.StudentId = grade.StudentId;
                existingGrade.Value = grade.Value;
                existingGrade.MentorComment = grade.MentorComment;
                existingGrade.CreatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["TaskItemId"] = new SelectList(_context.TaskItems, "Id", "Title", grade.TaskItemId);
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Email", grade.StudentId);

            grade.TaskItem = await _context.TaskItems.FindAsync(grade.TaskItemId);
            grade.Student = await _context.Users.FindAsync(grade.StudentId);

            return View(grade);
        }

        // GET: Grades/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.TaskItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _context.Grades.FindAsync(id);

            if (grade != null)
            {
                _context.Grades.Remove(grade);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool GradeExists(int id)
        {
            return _context.Grades.Any(e => e.Id == id);
        }
    }
}