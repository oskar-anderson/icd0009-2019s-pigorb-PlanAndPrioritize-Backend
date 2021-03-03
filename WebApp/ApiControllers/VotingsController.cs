using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VotingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Votings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voting>>> GetVotings()
        {
            return await _context.Votings.ToListAsync();
        }

        // GET: api/Votings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voting>> GetVoting(Guid id)
        {
            var voting = await _context.Votings.FindAsync(id);

            if (voting == null)
            {
                return NotFound();
            }

            return voting;
        }

        // PUT: api/Votings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoting(Guid id, Voting voting)
        {
            if (id != voting.Id)
            {
                return BadRequest();
            }

            _context.Entry(voting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VotingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Votings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Voting>> PostVoting(Voting voting)
        {
            _context.Votings.Add(voting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVoting", new { id = voting.Id }, voting);
        }

        // DELETE: api/Votings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoting(Guid id)
        {
            var voting = await _context.Votings.FindAsync(id);
            if (voting == null)
            {
                return NotFound();
            }

            _context.Votings.Remove(voting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VotingExists(Guid id)
        {
            return _context.Votings.Any(e => e.Id == id);
        }
    }
}
