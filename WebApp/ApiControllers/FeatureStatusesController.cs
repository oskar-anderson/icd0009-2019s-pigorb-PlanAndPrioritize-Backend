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
    public class FeatureStatusesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeatureStatusesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/FeatureStatuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeatureStatus>>> GetFeatureStatuses()
        {
            return await _context.FeatureStatuses.ToListAsync();
        }

        // GET: api/FeatureStatuses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeatureStatus>> GetFeatureStatus(Guid id)
        {
            var featureStatus = await _context.FeatureStatuses.FindAsync(id);

            if (featureStatus == null)
            {
                return NotFound();
            }

            return featureStatus;
        }

        // PUT: api/FeatureStatuses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeatureStatus(Guid id, FeatureStatus featureStatus)
        {
            if (id != featureStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(featureStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureStatusExists(id))
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

        // POST: api/FeatureStatuses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FeatureStatus>> PostFeatureStatus(FeatureStatus featureStatus)
        {
            _context.FeatureStatuses.Add(featureStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeatureStatus", new { id = featureStatus.Id }, featureStatus);
        }

        // DELETE: api/FeatureStatuses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeatureStatus(Guid id)
        {
            var featureStatus = await _context.FeatureStatuses.FindAsync(id);
            if (featureStatus == null)
            {
                return NotFound();
            }

            _context.FeatureStatuses.Remove(featureStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeatureStatusExists(Guid id)
        {
            return _context.FeatureStatuses.Any(e => e.Id == id);
        }
    }
}
