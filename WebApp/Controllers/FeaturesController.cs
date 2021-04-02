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
    public class FeaturesController : Controller
    {
        private readonly AppDbContext _context;

        public FeaturesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Features
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Features.Include(f => f.AppUser).Include(f => f.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Features/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features
                .Include(f => f.AppUser)
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // GET: Features/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Features/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Size,PriorityValue,Description,StartTime,EndTime,Duration,CategoryId,FeatureStatusId,AppUserId,TimeCreated,LastEdited,ChangeLog")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                feature.Id = Guid.NewGuid();
                _context.Add(feature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", feature.AppUserId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", feature.CategoryId);
            return View(feature);
        }

        // GET: Features/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", feature.AppUserId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", feature.CategoryId);
            return View(feature);
        }

        // POST: Features/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Size,PriorityValue,Description,StartTime,EndTime,Duration,CategoryId,FeatureStatusId,AppUserId,TimeCreated,LastEdited,ChangeLog")] Feature feature)
        {
            if (id != feature.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeatureExists(feature.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", feature.AppUserId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", feature.CategoryId);
            return View(feature);
        }

        // GET: Features/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features
                .Include(f => f.AppUser)
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // POST: Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var feature = await _context.Features.FindAsync(id);
            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeatureExists(Guid id)
        {
            return _context.Features.Any(e => e.Id == id);
        }
    }
}
