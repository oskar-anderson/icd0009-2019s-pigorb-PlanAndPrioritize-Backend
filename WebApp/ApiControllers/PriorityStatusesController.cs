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
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PriorityStatusesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PriorityStatusesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PriorityStatuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriorityStatus>>> GetPriorityStatuses()
        {
            return await _context.PriorityStatuses.ToListAsync();
        }

        // GET: api/PriorityStatuses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PriorityStatus>> GetPriorityStatus(Guid id)
        {
            var priorityStatus = await _context.PriorityStatuses.FindAsync(id);

            if (priorityStatus == null)
            {
                return NotFound();
            }

            return priorityStatus;
        }

        // PUT: api/PriorityStatuses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPriorityStatus(Guid id, PriorityStatus priorityStatus)
        {
            if (id != priorityStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(priorityStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriorityStatusExists(id))
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

        // POST: api/PriorityStatuses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PriorityStatus>> PostPriorityStatus(PriorityStatus priorityStatus)
        {
            _context.PriorityStatuses.Add(priorityStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPriorityStatus", new { id = priorityStatus.Id }, priorityStatus);
        }

        // DELETE: api/PriorityStatuses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriorityStatus(Guid id)
        {
            var priorityStatus = await _context.PriorityStatuses.FindAsync(id);
            if (priorityStatus == null)
            {
                return NotFound();
            }

            _context.PriorityStatuses.Remove(priorityStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PriorityStatusExists(Guid id)
        {
            return _context.PriorityStatuses.Any(e => e.Id == id);
        }
    }
}
