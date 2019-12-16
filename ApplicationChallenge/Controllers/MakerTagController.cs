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
    public class MakerTagController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public MakerTagController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/MakerTag
        [HttpGet]
        [Permission("MakerTag.OnGet")]
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
                    makertag.Interest = (makertag.Interest - (makertag.Interest / 10 * 1));
                    _context.SaveChanges();
                }
            }
        }

        // GET: api/MakerTag/5
        [HttpGet("{id}")]
        [Permission("MakerTag.OnGetID")]
        public async Task<ActionResult<MakerTag>> GetMakerTag(long id)
        {
            var makerTag = await _context.MakerTags.FindAsync(id);

            if (makerTag == null)
            {
                return NotFound();
            }

            return makerTag;
        }

        [HttpGet("gettags/{id}")]
        [Permission("MakerTags.OnGetIDIncludeTag")]
        public async Task<ActionResult<IEnumerable<MakerTag>>> GetMakerTags(long id)
        {
            var makerTags = await _context.MakerTags.Include(t => t.Tag).Where(b => b.MakerId == id).ToListAsync();

            if (makerTags == null)
            {
                return NotFound();
            }

            return makerTags;
        }

        // PUT: api/MakerTag/5
        [HttpPut("edittags/{id}")]
        public async Task<IActionResult> PutMakerTag(long id, MakerTag makerTag)
        {

            var makerTags = await _context.MakerTags.Where(o => o.MakerId == id).Include(t => t.Tag).ToListAsync();
            if (makerTags == null)
            {
                return NotFound();
            }
            foreach (var vmakerTag in makerTags)
            {
                _context.MakerTags.Remove(vmakerTag);

            }


            

            //_context.Add(new MakerTag({ }))

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
        public async Task<ActionResult<IEnumerable<MakerTag>>> DeleteMakerTag(long id)
        {
            var makerTags = await _context.MakerTags.Where(o => o.MakerId == id).ToListAsync();
            if (makerTags == null)
            {
                return NotFound();
            }
            foreach (var makerTag in makerTags)
            {
                _context.MakerTags.Remove(makerTag);

            }
            await _context.SaveChangesAsync();

            return makerTags;
        }

        // DELETE: api/MakerTag/makerid/5
        [HttpDelete("makerId/{makerId}")]
        public async Task<ActionResult<IEnumerable<MakerTag>>> DeleteMakerTagWhereMakerId(int makerId)
        {
            var makerTags = await _context.MakerTags.Where(m => m.MakerId == makerId).ToListAsync();
            if (makerTags == null)
            {
                return NotFound();
            }
            foreach (var makerTag in makerTags)
            {
                _context.MakerTags.Remove(makerTag);
            }
            await _context.SaveChangesAsync();

            return makerTags;
        }

        private bool MakerTagExists(long id)
        {
            return _context.MakerTags.Any(e => e.Id == id);
        }
    }
}
