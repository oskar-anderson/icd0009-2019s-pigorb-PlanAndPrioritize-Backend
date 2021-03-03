using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;

namespace WebApp.Controllers
{
    public class SubTasksController : Controller
    {
        private readonly AppDbContext _context;

        public SubTasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SubTasks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.SubTasks.Include(s => s.Feature);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SubTasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTask = await _context.SubTasks
                .Include(s => s.Feature)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subTask == null)
            {
                return NotFound();
            }

            return View(subTask);
        }

        // GET: SubTasks/Create
        public IActionResult Create()
        {
            ViewData["FeatureId"] = new SelectList(_context.Features, "Id", "Description");
            return View();
        }

        // POST: SubTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Heading,Description,FeatureId")] SubTask subTask)
        {
            if (ModelState.IsValid)
            {
                subTask.Id = Guid.NewGuid();
                _context.Add(subTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FeatureId"] = new SelectList(_context.Features, "Id", "Description", subTask.FeatureId);
            return View(subTask);
        }

        // GET: SubTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTask = await _context.SubTasks.FindAsync(id);
            if (subTask == null)
            {
                return NotFound();
            }
            ViewData["FeatureId"] = new SelectList(_context.Features, "Id", "Description", subTask.FeatureId);
            return View(subTask);
        }

        // POST: SubTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Heading,Description,FeatureId")] SubTask subTask)
        {
            if (id != subTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubTaskExists(subTask.Id))
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
            ViewData["FeatureId"] = new SelectList(_context.Features, "Id", "Description", subTask.FeatureId);
            return View(subTask);
        }

        // GET: SubTasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTask = await _context.SubTasks
                .Include(s => s.Feature)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subTask == null)
            {
                return NotFound();
            }

            return View(subTask);
        }

        // POST: SubTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subTask = await _context.SubTasks.FindAsync(id);
            _context.SubTasks.Remove(subTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubTaskExists(Guid id)
        {
            return _context.SubTasks.Any(e => e.Id == id);
        }
    }
}
