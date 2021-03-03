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
    public class VotingStatusesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VotingStatusesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/VotingStatuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VotingStatus>>> GetVotingStatuses()
        {
            return await _context.VotingStatuses.ToListAsync();
        }

        // GET: api/VotingStatuses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VotingStatus>> GetVotingStatus(Guid id)
        {
            var votingStatus = await _context.VotingStatuses.FindAsync(id);

            if (votingStatus == null)
            {
                return NotFound();
            }

            return votingStatus;
        }

        // PUT: api/VotingStatuses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVotingStatus(Guid id, VotingStatus votingStatus)
        {
            if (id != votingStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(votingStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VotingStatusExists(id))
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

        // POST: api/VotingStatuses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VotingStatus>> PostVotingStatus(VotingStatus votingStatus)
        {
            _context.VotingStatuses.Add(votingStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVotingStatus", new { id = votingStatus.Id }, votingStatus);
        }

        // DELETE: api/VotingStatuses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVotingStatus(Guid id)
        {
            var votingStatus = await _context.VotingStatuses.FindAsync(id);
            if (votingStatus == null)
            {
                return NotFound();
            }

            _context.VotingStatuses.Remove(votingStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VotingStatusExists(Guid id)
        {
            return _context.VotingStatuses.Any(e => e.Id == id);
        }
    }
}
