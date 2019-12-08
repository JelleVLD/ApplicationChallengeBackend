using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationChallenge.Models;

namespace ApplicationChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedrijfTagController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public BedrijfTagController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/BedrijfTag
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BedrijfTag>>> GetBedrijfTags()
        {
            return await _context.BedrijfTags.ToListAsync();
        }

        // GET: api/BedrijfTag/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BedrijfTag>> GetBedrijfTag(long id)
        {
            var bedrijfTag = await _context.BedrijfTags.FindAsync(id);

            if (bedrijfTag == null)
            {
                return NotFound();
            }

            return bedrijfTag;
        }

        // PUT: api/BedrijfTag/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBedrijfTag(long id, BedrijfTag bedrijfTag)
        {
            if (id != bedrijfTag.Id)
            {
                return BadRequest();
            }

            _context.Entry(bedrijfTag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BedrijfTagExists(id))
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

        // POST: api/BedrijfTag
        [HttpPost]
        public async Task<ActionResult<BedrijfTag>> PostBedrijfTag(BedrijfTag bedrijfTag)
        {
            _context.BedrijfTags.Add(bedrijfTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBedrijfTag", new { id = bedrijfTag.Id }, bedrijfTag);
        }

        // DELETE: api/BedrijfTag/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BedrijfTag>> DeleteBedrijfTag(long id)
        {
            var bedrijfTag = await _context.BedrijfTags.FindAsync(id);
            if (bedrijfTag == null)
            {
                return NotFound();
            }

            _context.BedrijfTags.Remove(bedrijfTag);
            await _context.SaveChangesAsync();

            return bedrijfTag;
        }

        private bool BedrijfTagExists(long id)
        {
            return _context.BedrijfTags.Any(e => e.Id == id);
        }
    }
}
