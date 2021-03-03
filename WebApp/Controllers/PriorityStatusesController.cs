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
    public class PriorityStatusesController : Controller
    {
        private readonly AppDbContext _context;

        public PriorityStatusesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PriorityStatuses
        public async Task<IActionResult> Index()
        {
            return View(await _context.PriorityStatuses.ToListAsync());
        }

        // GET: PriorityStatuses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priorityStatus = await _context.PriorityStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (priorityStatus == null)
            {
                return NotFound();
            }

            return View(priorityStatus);
        }

        // GET: PriorityStatuses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PriorityStatuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] PriorityStatus priorityStatus)
        {
            if (ModelState.IsValid)
            {
                priorityStatus.Id = Guid.NewGuid();
                _context.Add(priorityStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(priorityStatus);
        }

        // GET: PriorityStatuses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priorityStatus = await _context.PriorityStatuses.FindAsync(id);
            if (priorityStatus == null)
            {
                return NotFound();
            }
            return View(priorityStatus);
        }

        // POST: PriorityStatuses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description")] PriorityStatus priorityStatus)
        {
            if (id != priorityStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priorityStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriorityStatusExists(priorityStatus.Id))
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
            return View(priorityStatus);
        }

        // GET: PriorityStatuses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priorityStatus = await _context.PriorityStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (priorityStatus == null)
            {
                return NotFound();
            }

            return View(priorityStatus);
        }

        // POST: PriorityStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var priorityStatus = await _context.PriorityStatuses.FindAsync(id);
            _context.PriorityStatuses.Remove(priorityStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriorityStatusExists(Guid id)
        {
            return _context.PriorityStatuses.Any(e => e.Id == id);
        }
    }
}
