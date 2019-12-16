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
    public class BedrijfTagController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public BedrijfTagController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/BedrijfTag
        [HttpGet]
        [Permission("BedrijfTag.OnGet")]
        public async Task<ActionResult<IEnumerable<BedrijfTag>>> GetBedrijfTags()
        {
            return await _context.BedrijfTags.ToListAsync();
        }

        // GET: api/BedrijfTag/5
        [HttpGet("{id}")] [Permission("BedrijfTag.OnGetBedrijfID")]
        public async Task<ActionResult<IEnumerable<BedrijfTag>>> GetBedrijfTags(long id)
        {
            var bedrijfTags = await _context.BedrijfTags.Include(t => t.Tag).Where(b => b.BedrijfId == id).ToListAsync();

            if (bedrijfTags == null)
            {
                return NotFound();
            }

            return bedrijfTags;
        }

        // PUT: api/BedrijfTag/5
        [HttpPut("{id}")]
        [Permission("BedrijfTag.OnGetID")]
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
        [HttpPost] [Permission("BedrijfTag.OnCreate")]
        public async Task<ActionResult<BedrijfTag>> PostBedrijfTag(BedrijfTag bedrijfTag)
        {
            _context.BedrijfTags.Add(bedrijfTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBedrijfTag", new { id = bedrijfTag.Id }, bedrijfTag);
        }

        // DELETE: api/BedrijfTag/5
        [HttpDelete("{id}")] [Permission("BedrijfTag.OnDelete")]
        public async Task<ActionResult<IEnumerable<BedrijfTag>>> DeleteBedrijfTag(long id)
        {
            var bedrijfTags = await _context.BedrijfTags.Where(o => o.BedrijfId == id).Include(t => t.Tag).ToListAsync();
            if (bedrijfTags == null)
            {
                return NotFound();
            }
            foreach (var bedrijfTag in bedrijfTags)
            {
                _context.BedrijfTags.Remove(bedrijfTag);

            }
            await _context.SaveChangesAsync();

            return bedrijfTags;
        }

        private bool BedrijfTagExists(long id)
        {
            return _context.BedrijfTags.Any(e => e.Id == id);
        }
    }
}
