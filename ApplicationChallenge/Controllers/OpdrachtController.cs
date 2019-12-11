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
    public class OpdrachtController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public OpdrachtController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Opdracht
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Opdracht>>> GetOpdrachten()
        {
            return await _context.Opdrachten.ToListAsync();
        }

        // GET: api/Opdracht/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Opdracht>> GetOpdracht(long id)
        {
            var opdracht = await _context.Opdrachten.FindAsync(id);

            if (opdracht == null)
            {
                return NotFound();
            }

            return opdracht;
        }

        // PUT: api/Opdracht/5
        [HttpPut("{id}")] [Permission("Opdracht.OnPutID")]
        public async Task<IActionResult> PutOpdracht(long id, Opdracht opdracht)
        {
            if (id != opdracht.Id)
            {
                return BadRequest();
            }

            _context.Entry(opdracht).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpdrachtExists(id))
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

        // POST: api/Opdracht
        [HttpPost]
        [Permission("Opdracht.OnCreate")]
        public async Task<ActionResult<Opdracht>> PostOpdracht(Opdracht opdracht)
        {
            _context.Opdrachten.Add(opdracht);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOpdracht", new { id = opdracht.Id }, opdracht);
        }

        // DELETE: api/Opdracht/5
        [HttpDelete("{id}")] [Permission("Opdracht.OnDeleteID")]
        public async Task<ActionResult<Opdracht>> DeleteOpdracht(long id)
        {
            var opdracht = await _context.Opdrachten.FindAsync(id);
            if (opdracht == null)
            {
                return NotFound();
            }
            var opdrachtTags = await _context.OpdrachtTags.Where(o => o.OpdrachtId == id).ToListAsync();
            foreach (var opdrachtTag in opdrachtTags)
            {
                _context.OpdrachtTags.Remove(opdrachtTag);
            }
            _context.Opdrachten .Remove(opdracht);
            await _context.SaveChangesAsync();

            return opdracht;
        }

        private bool OpdrachtExists(long id)
        {
            return _context.Opdrachten.Any(e => e.Id == id);
        }
    }
}
