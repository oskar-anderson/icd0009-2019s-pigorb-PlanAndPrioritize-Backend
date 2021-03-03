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
    public class SubTasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubTasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SubTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubTask>>> GetSubTasks()
        {
            return await _context.SubTasks.ToListAsync();
        }

        // GET: api/SubTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubTask>> GetSubTask(Guid id)
        {
            var subTask = await _context.SubTasks.FindAsync(id);

            if (subTask == null)
            {
                return NotFound();
            }

            return subTask;
        }

        // PUT: api/SubTasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubTask(Guid id, SubTask subTask)
        {
            if (id != subTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(subTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubTaskExists(id))
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

        // POST: api/SubTasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubTask>> PostSubTask(SubTask subTask)
        {
            _context.SubTasks.Add(subTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubTask", new { id = subTask.Id }, subTask);
        }

        // DELETE: api/SubTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubTask(Guid id)
        {
            var subTask = await _context.SubTasks.FindAsync(id);
            if (subTask == null)
            {
                return NotFound();
            }

            _context.SubTasks.Remove(subTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubTaskExists(Guid id)
        {
            return _context.SubTasks.Any(e => e.Id == id);
        }
    }
}
