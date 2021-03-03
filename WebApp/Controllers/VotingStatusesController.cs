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
    public class VotingStatusesController : Controller
    {
        private readonly AppDbContext _context;

        public VotingStatusesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: VotingStatuses
        public async Task<IActionResult> Index()
        {
            return View(await _context.VotingStatuses.ToListAsync());
        }

        // GET: VotingStatuses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var votingStatus = await _context.VotingStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (votingStatus == null)
            {
                return NotFound();
            }

            return View(votingStatus);
        }

        // GET: VotingStatuses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VotingStatuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] VotingStatus votingStatus)
        {
            if (ModelState.IsValid)
            {
                votingStatus.Id = Guid.NewGuid();
                _context.Add(votingStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(votingStatus);
        }

        // GET: VotingStatuses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var votingStatus = await _context.VotingStatuses.FindAsync(id);
            if (votingStatus == null)
            {
                return NotFound();
            }
            return View(votingStatus);
        }

        // POST: VotingStatuses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description")] VotingStatus votingStatus)
        {
            if (id != votingStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(votingStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VotingStatusExists(votingStatus.Id))
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
            return View(votingStatus);
        }

        // GET: VotingStatuses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var votingStatus = await _context.VotingStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (votingStatus == null)
            {
                return NotFound();
            }

            return View(votingStatus);
        }

        // POST: VotingStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var votingStatus = await _context.VotingStatuses.FindAsync(id);
            _context.VotingStatuses.Remove(votingStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VotingStatusExists(Guid id)
        {
            return _context.VotingStatuses.Any(e => e.Id == id);
        }
    }
}
