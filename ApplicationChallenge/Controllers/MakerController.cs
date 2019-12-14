using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationChallenge.Models;
using ApplicationChallenge.Attributes;

namespace ApplicationChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakerController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public MakerController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Maker
        [HttpGet] [Permission("Maker.OnGet")]
        public async Task<ActionResult<IEnumerable<Maker>>> GetMakers()
        {
            return await _context.Makers.ToListAsync();
        }

        // GET: api/Maker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Maker>> GetMaker(long id)
        {
            var maker = await _context.Makers.FindAsync(id);

            if (maker == null)
            {
                return NotFound();
            }

            return maker;
        }

        // PUT: api/Maker/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaker(long id, Maker maker)
        {
            if (id != maker.Id)
            {
                return BadRequest();
            }

            _context.Entry(maker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MakerExists(id))
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

        // POST: api/Maker
        [HttpPost]
        public async Task<ActionResult<Maker>> PostMaker(Maker maker)
        {
            _context.Makers.Add(maker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaker", new { id = maker.Id }, maker);
        }

        // DELETE: api/Maker/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Maker>> DeleteMaker(long id)
        {
            var maker = await _context.Makers.FindAsync(id);
            if (maker == null)
            {
                return NotFound();
            }

            _context.Makers.Remove(maker);
            await _context.SaveChangesAsync();

            return maker;
        }

        private bool MakerExists(long id)
        {
            return _context.Makers.Any(e => e.Id == id);
        }
    }
}
