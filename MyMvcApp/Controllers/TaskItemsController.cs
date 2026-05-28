using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskItemsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TaskItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TaskItems
                .Include(t => t.InternshipGroup)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TaskItems/MyTasks
        public async Task<IActionResult> MyTasks()
        {
            var userId = _userManager.GetUserId(User);

            var tasks = await _context.TaskItems
                .Include(t => t.InternshipGroup)
                .Include(t => t.AssignedToUser)
                .Where(t => t.AssignedToUserId == userId)
                .ToListAsync();

            return View(tasks);
        }

        // GET: TaskItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .Include(t => t.InternshipGroup)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // GET: TaskItems/Create
        public async Task<IActionResult> Create()
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");

            ViewData["AssignedToUserId"] = new SelectList(students, "Id", "Email");
            ViewData["InternshipGroupId"] = new SelectList(_context.InternshipGroups, "Id", "Name");
            ViewData["Priority"] = new SelectList(Enum.GetValues(typeof(TaskPriority)));
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(MyMvcApp.Models.TaskStatus)));

            return View();
        }

        // POST: TaskItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Description,Priority,Status,Deadline,AssignedToUserId,InternshipGroupId")]
            TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                taskItem.CreatedAt = DateTime.Now;
                taskItem.CreatedByUserId = _userManager.GetUserId(User);

                _context.Add(taskItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var students = await _userManager.GetUsersInRoleAsync("Student");

            ViewData["AssignedToUserId"] =
                new SelectList(students, "Id", "Email", taskItem.AssignedToUserId);

            ViewData["InternshipGroupId"] =
                new SelectList(_context.InternshipGroups, "Id", "Name", taskItem.InternshipGroupId);

            ViewData["Priority"] =
                new SelectList(Enum.GetValues(typeof(TaskPriority)), taskItem.Priority);

            ViewData["Status"] =
                new SelectList(Enum.GetValues(typeof(MyMvcApp.Models.TaskStatus)), taskItem.Status);

            return View(taskItem);
        }

        // GET: TaskItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            var students = await _userManager.GetUsersInRoleAsync("Student");

            ViewData["AssignedToUserId"] =
                new SelectList(students, "Id", "Email", taskItem.AssignedToUserId);

            ViewData["InternshipGroupId"] =
                new SelectList(
                    _context.InternshipGroups,
                    "Id",
                    "Name",
                    taskItem.InternshipGroupId);

            ViewData["Priority"] =
                new SelectList(
                    Enum.GetValues(typeof(TaskPriority)),
                    taskItem.Priority);

            ViewData["Status"] =
                new SelectList(
                    Enum.GetValues(typeof(MyMvcApp.Models.TaskStatus)),
                    taskItem.Status);

            return View(taskItem);
        }

        // POST: TaskItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Description,Priority,Status,Deadline,AssignedToUserId,InternshipGroupId")]
            TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTask =
                        await _context.TaskItems.AsNoTracking()
                        .FirstOrDefaultAsync(t => t.Id == id);

                    if (existingTask == null)
                    {
                        return NotFound();
                    }

                    taskItem.CreatedAt = existingTask.CreatedAt;
                    taskItem.CreatedByUserId = existingTask.CreatedByUserId;

                    _context.Update(taskItem);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            var students = await _userManager.GetUsersInRoleAsync("Student");

            ViewData["AssignedToUserId"] =
                new SelectList(students, "Id", "Email", taskItem.AssignedToUserId);

            ViewData["InternshipGroupId"] =
                new SelectList(
                    _context.InternshipGroups,
                    "Id",
                    "Name",
                    taskItem.InternshipGroupId);

            ViewData["Priority"] =
                new SelectList(
                    Enum.GetValues(typeof(TaskPriority)),
                    taskItem.Priority);

            ViewData["Status"] =
                new SelectList(
                    Enum.GetValues(typeof(MyMvcApp.Models.TaskStatus)),
                    taskItem.Status);

            return View(taskItem);
        }

        // GET: TaskItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .Include(t => t.InternshipGroup)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}