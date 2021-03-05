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
    public class UsersFeaturePrioritiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersFeaturePrioritiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UsersFeaturePriorities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersFeaturePriority>>> GetUsersFeaturePriorities()
        {
            return await _context.UsersFeaturePriorities.ToListAsync();
        }

        // GET: api/UsersFeaturePriorities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersFeaturePriority>> GetUsersFeaturePriority(Guid id)
        {
            var usersFeaturePriority = await _context.UsersFeaturePriorities.FindAsync(id);

            if (usersFeaturePriority == null)
            {
                return NotFound();
            }

            return usersFeaturePriority;
        }

        // PUT: api/UsersFeaturePriorities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsersFeaturePriority(Guid id, UsersFeaturePriority usersFeaturePriority)
        {
            if (id != usersFeaturePriority.Id)
            {
                return BadRequest();
            }

            _context.Entry(usersFeaturePriority).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersFeaturePriorityExists(id))
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

        // POST: api/UsersFeaturePriorities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsersFeaturePriority>> PostUsersFeaturePriority(UsersFeaturePriority usersFeaturePriority)
        {
            _context.UsersFeaturePriorities.Add(usersFeaturePriority);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsersFeaturePriority", new { id = usersFeaturePriority.Id }, usersFeaturePriority);
        }

        // DELETE: api/UsersFeaturePriorities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsersFeaturePriority(Guid id)
        {
            var usersFeaturePriority = await _context.UsersFeaturePriorities.FindAsync(id);
            if (usersFeaturePriority == null)
            {
                return NotFound();
            }

            _context.UsersFeaturePriorities.Remove(usersFeaturePriority);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersFeaturePriorityExists(Guid id)
        {
            return _context.UsersFeaturePriorities.Any(e => e.Id == id);
        }
    }
}
