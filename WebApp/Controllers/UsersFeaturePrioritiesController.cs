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
    public class UsersFeaturePrioritiesController : Controller
    {
        private readonly AppDbContext _context;

        public UsersFeaturePrioritiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UsersFeaturePriorities
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UsersFeaturePriorities.Include(u => u.AppUser).Include(u => u.FeatureInVoting);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UsersFeaturePriorities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersFeaturePriority = await _context.UsersFeaturePriorities
                .Include(u => u.AppUser)
                .Include(u => u.FeatureInVoting)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersFeaturePriority == null)
            {
                return NotFound();
            }

            return View(usersFeaturePriority);
        }

        // GET: UsersFeaturePriorities/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName");
            ViewData["FeatureInVotingId"] = new SelectList(_context.FeatureInVotings, "Id", "Id");
            return View();
        }

        // POST: UsersFeaturePriorities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BusinessValue,TimeCriticality,RiskOrOpportunity,Size,PriorityValue,AppUserId,FeatureInVotingId,PriorityStatusId")] UsersFeaturePriority usersFeaturePriority)
        {
            if (ModelState.IsValid)
            {
                usersFeaturePriority.Id = Guid.NewGuid();
                _context.Add(usersFeaturePriority);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", usersFeaturePriority.AppUserId);
            ViewData["FeatureInVotingId"] = new SelectList(_context.FeatureInVotings, "Id", "Id", usersFeaturePriority.FeatureInVotingId);
            return View(usersFeaturePriority);
        }

        // GET: UsersFeaturePriorities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersFeaturePriority = await _context.UsersFeaturePriorities.FindAsync(id);
            if (usersFeaturePriority == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", usersFeaturePriority.AppUserId);
            ViewData["FeatureInVotingId"] = new SelectList(_context.FeatureInVotings, "Id", "Id", usersFeaturePriority.FeatureInVotingId);
            return View(usersFeaturePriority);
        }

        // POST: UsersFeaturePriorities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,BusinessValue,TimeCriticality,RiskOrOpportunity,Size,PriorityValue,AppUserId,FeatureInVotingId,PriorityStatusId")] UsersFeaturePriority usersFeaturePriority)
        {
            if (id != usersFeaturePriority.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersFeaturePriority);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersFeaturePriorityExists(usersFeaturePriority.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", usersFeaturePriority.AppUserId);
            ViewData["FeatureInVotingId"] = new SelectList(_context.FeatureInVotings, "Id", "Id", usersFeaturePriority.FeatureInVotingId);
            return View(usersFeaturePriority);
        }

        // GET: UsersFeaturePriorities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersFeaturePriority = await _context.UsersFeaturePriorities
                .Include(u => u.AppUser)
                .Include(u => u.FeatureInVoting)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersFeaturePriority == null)
            {
                return NotFound();
            }

            return View(usersFeaturePriority);
        }

        // POST: UsersFeaturePriorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var usersFeaturePriority = await _context.UsersFeaturePriorities.FindAsync(id);
            _context.UsersFeaturePriorities.Remove(usersFeaturePriority);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersFeaturePriorityExists(Guid id)
        {
            return _context.UsersFeaturePriorities.Any(e => e.Id == id);
        }
    }
}
