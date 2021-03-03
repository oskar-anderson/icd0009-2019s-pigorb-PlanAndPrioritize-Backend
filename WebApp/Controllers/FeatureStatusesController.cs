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
    public class FeatureStatusesController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureStatusesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FeatureStatuses
        public async Task<IActionResult> Index()
        {
            return View(await _context.FeatureStatuses.ToListAsync());
        }

        // GET: FeatureStatuses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featureStatus = await _context.FeatureStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (featureStatus == null)
            {
                return NotFound();
            }

            return View(featureStatus);
        }

        // GET: FeatureStatuses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeatureStatuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] FeatureStatus featureStatus)
        {
            if (ModelState.IsValid)
            {
                featureStatus.Id = Guid.NewGuid();
                _context.Add(featureStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(featureStatus);
        }

        // GET: FeatureStatuses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featureStatus = await _context.FeatureStatuses.FindAsync(id);
            if (featureStatus == null)
            {
                return NotFound();
            }
            return View(featureStatus);
        }

        // POST: FeatureStatuses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description")] FeatureStatus featureStatus)
        {
            if (id != featureStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(featureStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeatureStatusExists(featureStatus.Id))
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
            return View(featureStatus);
        }

        // GET: FeatureStatuses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featureStatus = await _context.FeatureStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (featureStatus == null)
            {
                return NotFound();
            }

            return View(featureStatus);
        }

        // POST: FeatureStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var featureStatus = await _context.FeatureStatuses.FindAsync(id);
            _context.FeatureStatuses.Remove(featureStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeatureStatusExists(Guid id)
        {
            return _context.FeatureStatuses.Any(e => e.Id == id);
        }
    }
}
