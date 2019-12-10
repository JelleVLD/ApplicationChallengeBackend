using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationChallenge.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillMakerController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public SkillMakerController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/SkillMaker
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillMaker>>> GetSkillMakers()
        {
            return await _context.SkillMakers.ToListAsync();
        }

        // GET: api/SkillMaker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SkillMaker>> GetSkillMaker(long id)
        {
            var skillMaker = await _context.SkillMakers.FindAsync(id);

            if (skillMaker == null)
            {
                return NotFound();
            }

            return skillMaker;
        }

        // PUT: api/SkillMaker/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkillMaker(long id, SkillMaker skillMaker)
        {
            if (id != skillMaker.Id)
            {
                return BadRequest();
            }

            _context.Entry(skillMaker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillMakerExists(id))
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

        // POST: api/SkillMaker
        [HttpPost]
        public async Task<ActionResult<SkillMaker>> PostSkillMaker(SkillMaker skillMaker)
        {
            _context.SkillMakers.Add(skillMaker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSkillMaker", new { id = skillMaker.Id }, skillMaker);
        }

        // DELETE: api/SkillMaker/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SkillMaker>> DeleteSkillMaker(long id)
        {
            var skillMaker = await _context.SkillMakers.FindAsync(id);
            if (skillMaker == null)
            {
                return NotFound();
            }

            _context.SkillMakers.Remove(skillMaker);
            await _context.SaveChangesAsync();

            return skillMaker;
        }

        private bool SkillMakerExists(long id)
        {
            return _context.SkillMakers.Any(e => e.Id == id);
        }
    }
}
