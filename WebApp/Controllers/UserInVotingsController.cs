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
    public class UserInVotingsController : Controller
    {
        private readonly AppDbContext _context;

        public UserInVotingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserInVotings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserInVotings.Include(u => u.AppUser).Include(u => u.Voting);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserInVotings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInVoting = await _context.UserInVotings
                .Include(u => u.AppUser)
                .Include(u => u.Voting)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInVoting == null)
            {
                return NotFound();
            }

            return View(userInVoting);
        }

        // GET: UserInVotings/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName");
            ViewData["VotingId"] = new SelectList(_context.Votings, "Id", "Title");
            return View();
        }

        // POST: UserInVotings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUserId,VotingId")] UserInVoting userInVoting)
        {
            if (ModelState.IsValid)
            {
                userInVoting.Id = Guid.NewGuid();
                _context.Add(userInVoting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", userInVoting.AppUserId);
            ViewData["VotingId"] = new SelectList(_context.Votings, "Id", "Title", userInVoting.VotingId);
            return View(userInVoting);
        }

        // GET: UserInVotings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInVoting = await _context.UserInVotings.FindAsync(id);
            if (userInVoting == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", userInVoting.AppUserId);
            ViewData["VotingId"] = new SelectList(_context.Votings, "Id", "Title", userInVoting.VotingId);
            return View(userInVoting);
        }

        // POST: UserInVotings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AppUserId,VotingId")] UserInVoting userInVoting)
        {
            if (id != userInVoting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInVoting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInVotingExists(userInVoting.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", userInVoting.AppUserId);
            ViewData["VotingId"] = new SelectList(_context.Votings, "Id", "Title", userInVoting.VotingId);
            return View(userInVoting);
        }

        // GET: UserInVotings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInVoting = await _context.UserInVotings
                .Include(u => u.AppUser)
                .Include(u => u.Voting)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInVoting == null)
            {
                return NotFound();
            }

            return View(userInVoting);
        }

        // POST: UserInVotings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userInVoting = await _context.UserInVotings.FindAsync(id);
            _context.UserInVotings.Remove(userInVoting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInVotingExists(Guid id)
        {
            return _context.UserInVotings.Any(e => e.Id == id);
        }
    }
}
