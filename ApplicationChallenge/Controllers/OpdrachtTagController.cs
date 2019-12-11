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
    public class OpdrachtTagController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public OpdrachtTagController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/OpdrachtTag
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OpdrachtTag>>> GetOpdrachtTags()
        {
            return await _context.OpdrachtTags.ToListAsync();
        }

        // GET: api/OpdrachtTag/5
        [HttpGet("{id}")][Permission("OpdrachtTag.OnGetBedrijfID")]
        public async Task<ActionResult<IEnumerable<OpdrachtTag>>> GetOpdrachtTags(long id)
        {
            var opdrachtTags = await _context.OpdrachtTags.Include(t=> t.Tag).Where(o=>o.OpdrachtId == id).ToListAsync();

            if (opdrachtTags == null)
            {
                return NotFound();
            }

            return opdrachtTags;
        }

        // PUT: api/OpdrachtTag/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOpdrachtTag(long id, OpdrachtTag opdrachtTag)
        {
            if (id != opdrachtTag.Id)
            {
                return BadRequest();
            }

            _context.Entry(opdrachtTag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpdrachtTagExists(id))
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

        // POST: api/OpdrachtTag
        [HttpPost] [Permission("OpdrachtTag.OnCreate")]
        public async Task<ActionResult<OpdrachtTag>> PostOpdrachtTag(OpdrachtTag opdrachtTag)
        {
            _context.OpdrachtTags.Add(opdrachtTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOpdrachtTag", new { id = opdrachtTag.Id }, opdrachtTag);
        }

        // DELETE: api/OpdrachtTag/5
        [HttpDelete("{id}")] [Permission("OpdrachtTag.OnDelete")]
        public async Task<ActionResult<IEnumerable<OpdrachtTag>>> DeleteOpdrachtTag(long id)
        {
            var opdrachtTags = await _context.OpdrachtTags.Where(o=>o.OpdrachtId == id).Include(t=>t.Tag).ToListAsync();
            if (opdrachtTags == null)
            {
                return NotFound();
            }
            foreach (var opdrachtTag in opdrachtTags)
            {
                _context.OpdrachtTags.Remove(opdrachtTag);
                _context.Tags.Remove(opdrachtTag.Tag);

            }
            await _context.SaveChangesAsync();

            return opdrachtTags;
        }

        private bool OpdrachtTagExists(long id)
        {
            return _context.OpdrachtTags.Any(e => e.Id == id);
        }
    }
}
