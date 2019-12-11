using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationChallenge.Models;
using ApplicationChallenge.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationChallenge.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BedrijfController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public BedrijfController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Bedrijf
        [HttpGet] [Permission("Bedrijf.onGet")]
        public async Task<ActionResult<IEnumerable<Bedrijf>>> GetBedrijven()
        {
            return await _context.Bedrijven.ToListAsync();
        }

        // GET: api/Bedrijf/5
        [HttpGet("{id}")] [Permission("Bedrijf.OnGetID")]
        public async Task<ActionResult<Bedrijf>> GetBedrijf(long id)
        {
            var bedrijf = await _context.Bedrijven.Include(o=> o.Opdrachten).Where(i => i.Id == id).FirstOrDefaultAsync();

            if (bedrijf == null)
            {
                return NotFound();
            }

            return bedrijf;
        }

        // PUT: api/Bedrijf/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBedrijf(long id, Bedrijf bedrijf)
        {
            if (id != bedrijf.Id)
            {
                return BadRequest();
            }

            _context.Entry(bedrijf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BedrijfExists(id))
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

        // POST: api/Bedrijf
        [HttpPost]
        public async Task<ActionResult<Bedrijf>> PostBedrijf(Bedrijf bedrijf)
        {
            _context.Bedrijven.Add(bedrijf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBedrijf", new { id = bedrijf.Id }, bedrijf);
        }

        // DELETE: api/Bedrijf/5
        [HttpDelete("{id}")] [Permission("Bedrijf.OnDeleteID")]
        public async Task<ActionResult<Bedrijf>> DeleteBedrijf(long id)
        {
            var bedrijf = await _context.Bedrijven.FindAsync(id);
            if (bedrijf == null)
            {
                return NotFound();
            }

            _context.Bedrijven.Remove(bedrijf);
            await _context.SaveChangesAsync();

            return bedrijf;
        }

        private bool BedrijfExists(long id)
        {
            return _context.Bedrijven.Any(e => e.Id == id);
        }
    }
}
