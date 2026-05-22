using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    public class InternshipGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InternshipGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InternshipGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.InternshipGroups.ToListAsync());
        }

        // GET: InternshipGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipGroup = await _context.InternshipGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (internshipGroup == null)
            {
                return NotFound();
            }

            return View(internshipGroup);
        }

        // GET: InternshipGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InternshipGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreatedAt")] InternshipGroup internshipGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(internshipGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(internshipGroup);
        }

        // GET: InternshipGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipGroup = await _context.InternshipGroups.FindAsync(id);
            if (internshipGroup == null)
            {
                return NotFound();
            }
            return View(internshipGroup);
        }

        // POST: InternshipGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedAt")] InternshipGroup internshipGroup)
        {
            if (id != internshipGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(internshipGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternshipGroupExists(internshipGroup.Id))
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
            return View(internshipGroup);
        }

        // GET: InternshipGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipGroup = await _context.InternshipGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (internshipGroup == null)
            {
                return NotFound();
            }

            return View(internshipGroup);
        }

        // POST: InternshipGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var internshipGroup = await _context.InternshipGroups.FindAsync(id);
            if (internshipGroup != null)
            {
                _context.InternshipGroups.Remove(internshipGroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InternshipGroupExists(int id)
        {
            return _context.InternshipGroups.Any(e => e.Id == id);
        }
    }
}
