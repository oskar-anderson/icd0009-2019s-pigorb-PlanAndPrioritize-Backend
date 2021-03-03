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
    public class FeatureInVotingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeatureInVotingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/FeatureInVotings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeatureInVoting>>> GetFeatureInVotings()
        {
            return await _context.FeatureInVotings.ToListAsync();
        }

        // GET: api/FeatureInVotings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeatureInVoting>> GetFeatureInVoting(Guid id)
        {
            var featureInVoting = await _context.FeatureInVotings.FindAsync(id);

            if (featureInVoting == null)
            {
                return NotFound();
            }

            return featureInVoting;
        }

        // PUT: api/FeatureInVotings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeatureInVoting(Guid id, FeatureInVoting featureInVoting)
        {
            if (id != featureInVoting.Id)
            {
                return BadRequest();
            }

            _context.Entry(featureInVoting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureInVotingExists(id))
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

        // POST: api/FeatureInVotings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FeatureInVoting>> PostFeatureInVoting(FeatureInVoting featureInVoting)
        {
            _context.FeatureInVotings.Add(featureInVoting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeatureInVoting", new { id = featureInVoting.Id }, featureInVoting);
        }

        // DELETE: api/FeatureInVotings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeatureInVoting(Guid id)
        {
            var featureInVoting = await _context.FeatureInVotings.FindAsync(id);
            if (featureInVoting == null)
            {
                return NotFound();
            }

            _context.FeatureInVotings.Remove(featureInVoting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeatureInVotingExists(Guid id)
        {
            return _context.FeatureInVotings.Any(e => e.Id == id);
        }
    }
}
