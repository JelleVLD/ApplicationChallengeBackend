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
    public class MakerTagController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public MakerTagController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/MakerTag
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MakerTag>>> GetMakerTags()
        {
            return await _context.MakerTags.ToListAsync();
        }

        // GET: api/MakerTag
        [HttpGet("lowerInterest")]
        public void LowerInterests()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "GebruikerId").Value;
            var makertags = _context.MakerTags.Where(x => x.MakerId == Convert.ToInt64(id)).ToList();

            foreach(var makertag in makertags)
            {
                if (makertag.SelfSet == false)
                {
                    _context.Entry(makertag).State = EntityState.Modified;
                    makertag.Interest = (makertag.Interest - (makertag.Interest / 10 * 3));
                    _context.SaveChanges();
                }
            }
        }

        // GET: api/MakerTag/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MakerTag>> GetMakerTag(long id)
        {
            var makerTag = await _context.MakerTags.FindAsync(id);

            if (makerTag == null)
            {
                return NotFound();
            }

            return makerTag;
        }

        // PUT: api/MakerTag/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMakerTag(long id, MakerTag makerTag)
        {
            if (id != makerTag.Id)
            {
                return BadRequest();
            }

            _context.Entry(makerTag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MakerTagExists(id))
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

        // POST: api/MakerTag
        [HttpPost]
        public async Task<ActionResult<MakerTag>> PostMakerTag(MakerTag makerTag)
        {
            _context.MakerTags.Add(makerTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMakerTag", new { id = makerTag.Id }, makerTag);
        }

        // DELETE: api/MakerTag/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MakerTag>> DeleteMakerTag(long id)
        {
            var makerTag = await _context.MakerTags.FindAsync(id);
            if (makerTag == null)
            {
                return NotFound();
            }

            _context.MakerTags.Remove(makerTag);
            await _context.SaveChangesAsync();

            return makerTag;
        }

        private bool MakerTagExists(long id)
        {
            return _context.MakerTags.Any(e => e.Id == id);
        }
    }
}
