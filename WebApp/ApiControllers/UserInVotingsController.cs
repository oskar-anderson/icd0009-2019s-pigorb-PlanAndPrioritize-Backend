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
    public class UserInVotingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserInVotingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserInVotings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInVoting>>> GetUserInVotings()
        {
            return await _context.UserInVotings.ToListAsync();
        }

        // GET: api/UserInVotings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInVoting>> GetUserInVoting(Guid id)
        {
            var userInVoting = await _context.UserInVotings.FindAsync(id);

            if (userInVoting == null)
            {
                return NotFound();
            }

            return userInVoting;
        }

        // PUT: api/UserInVotings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInVoting(Guid id, UserInVoting userInVoting)
        {
            if (id != userInVoting.Id)
            {
                return BadRequest();
            }

            _context.Entry(userInVoting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInVotingExists(id))
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

        // POST: api/UserInVotings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserInVoting>> PostUserInVoting(UserInVoting userInVoting)
        {
            _context.UserInVotings.Add(userInVoting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInVoting", new { id = userInVoting.Id }, userInVoting);
        }

        // DELETE: api/UserInVotings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserInVoting(Guid id)
        {
            var userInVoting = await _context.UserInVotings.FindAsync(id);
            if (userInVoting == null)
            {
                return NotFound();
            }

            _context.UserInVotings.Remove(userInVoting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserInVotingExists(Guid id)
        {
            return _context.UserInVotings.Any(e => e.Id == id);
        }
    }
}
