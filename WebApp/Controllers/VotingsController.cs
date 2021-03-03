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
    public class VotingsController : Controller
    {
        private readonly AppDbContext _context;

        public VotingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Votings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Votings.Include(v => v.VotingStatus);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Votings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voting = await _context.Votings
                .Include(v => v.VotingStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voting == null)
            {
                return NotFound();
            }

            return View(voting);
        }

        // GET: Votings/Create
        public IActionResult Create()
        {
            ViewData["VotingStatusId"] = new SelectList(_context.VotingStatuses, "Id", "Name");
            return View();
        }

        // POST: Votings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartTime,EndTime,VotingStatusId")] Voting voting)
        {
            if (ModelState.IsValid)
            {
                voting.Id = Guid.NewGuid();
                _context.Add(voting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VotingStatusId"] = new SelectList(_context.VotingStatuses, "Id", "Name", voting.VotingStatusId);
            return View(voting);
        }

        // GET: Votings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voting = await _context.Votings.FindAsync(id);
            if (voting == null)
            {
                return NotFound();
            }
            ViewData["VotingStatusId"] = new SelectList(_context.VotingStatuses, "Id", "Name", voting.VotingStatusId);
            return View(voting);
        }

        // POST: Votings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,StartTime,EndTime,VotingStatusId")] Voting voting)
        {
            if (id != voting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VotingExists(voting.Id))
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
            ViewData["VotingStatusId"] = new SelectList(_context.VotingStatuses, "Id", "Name", voting.VotingStatusId);
            return View(voting);
        }

        // GET: Votings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voting = await _context.Votings
                .Include(v => v.VotingStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voting == null)
            {
                return NotFound();
            }

            return View(voting);
        }

        // POST: Votings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var voting = await _context.Votings.FindAsync(id);
            _context.Votings.Remove(voting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VotingExists(Guid id)
        {
            return _context.Votings.Any(e => e.Id == id);
        }
    }
}
