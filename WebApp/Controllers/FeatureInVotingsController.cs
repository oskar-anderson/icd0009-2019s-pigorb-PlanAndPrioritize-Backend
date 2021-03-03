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
    public class FeatureInVotingsController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureInVotingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FeatureInVotings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.FeatureInVotings.Include(f => f.Feature).Include(f => f.Voting);
            return View(await appDbContext.ToListAsync());
        }

        // GET: FeatureInVotings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featureInVoting = await _context.FeatureInVotings
                .Include(f => f.Feature)
                .Include(f => f.Voting)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (featureInVoting == null)
            {
                return NotFound();
            }

            return View(featureInVoting);
        }

        // GET: FeatureInVotings/Create
        public IActionResult Create()
        {
            ViewData["FeatureId"] = new SelectList(_context.Features, "Id", "Title");
            ViewData["VotingId"] = new SelectList(_context.Votings, "Id", "Title");
            return View();
        }

        // POST: FeatureInVotings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AverageBusinessValue,AverageTimeCriticality,AverageRiskOrOpportunity,AverageSize,AveragePriorityValue,VotingId,FeatureId")] FeatureInVoting featureInVoting)
        {
            if (ModelState.IsValid)
            {
                featureInVoting.Id = Guid.NewGuid();
                _context.Add(featureInVoting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FeatureId"] = new SelectList(_context.Features, "Id", "Title", featureInVoting.FeatureId);
            ViewData["VotingId"] = new SelectList(_context.Votings, "Id", "Title", featureInVoting.VotingId);
            return View(featureInVoting);
        }

        // GET: FeatureInVotings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featureInVoting = await _context.FeatureInVotings.FindAsync(id);
            if (featureInVoting == null)
            {
                return NotFound();
            }
            ViewData["FeatureId"] = new SelectList(_context.Features, "Id", "Title", featureInVoting.FeatureId);
            ViewData["VotingId"] = new SelectList(_context.Votings, "Id", "Title", featureInVoting.VotingId);
            return View(featureInVoting);
        }

        // POST: FeatureInVotings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AverageBusinessValue,AverageTimeCriticality,AverageRiskOrOpportunity,AverageSize,AveragePriorityValue,VotingId,FeatureId")] FeatureInVoting featureInVoting)
        {
            if (id != featureInVoting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(featureInVoting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeatureInVotingExists(featureInVoting.Id))
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
            ViewData["FeatureId"] = new SelectList(_context.Features, "Id", "Title", featureInVoting.FeatureId);
            ViewData["VotingId"] = new SelectList(_context.Votings, "Id", "Title", featureInVoting.VotingId);
            return View(featureInVoting);
        }

        // GET: FeatureInVotings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featureInVoting = await _context.FeatureInVotings
                .Include(f => f.Feature)
                .Include(f => f.Voting)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (featureInVoting == null)
            {
                return NotFound();
            }

            return View(featureInVoting);
        }

        // POST: FeatureInVotings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var featureInVoting = await _context.FeatureInVotings.FindAsync(id);
            _context.FeatureInVotings.Remove(featureInVoting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeatureInVotingExists(Guid id)
        {
            return _context.FeatureInVotings.Any(e => e.Id == id);
        }
    }
}
