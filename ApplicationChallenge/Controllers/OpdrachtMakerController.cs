﻿using System;
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
    public class OpdrachtMakerController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public OpdrachtMakerController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/OpdrachtMaker
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OpdrachtMaker>>> GetOpdrachtMakers()
        {
            return await _context.OpdrachtMakers.ToListAsync();
        }

        // GET: api/OpdrachtMaker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OpdrachtMaker>> GetOpdrachtMaker(long id)
        {
            var opdrachtMaker = await _context.OpdrachtMakers.FindAsync(id);

            if (opdrachtMaker == null)
            {
                return NotFound();
            }

            return opdrachtMaker;
        }

        // PUT: api/OpdrachtMaker/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOpdrachtMaker(long id, OpdrachtMaker opdrachtMaker)
        {
            if (id != opdrachtMaker.Id)
            {
                return BadRequest();
            }

            _context.Entry(opdrachtMaker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpdrachtMakerExists(id))
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

        // POST: api/OpdrachtMaker
        [HttpPost]
        public async Task<ActionResult<OpdrachtMaker>> PostOpdrachtMaker(OpdrachtMaker opdrachtMaker)
        {
            _context.OpdrachtMakers.Add(opdrachtMaker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOpdrachtMaker", new { id = opdrachtMaker.Id }, opdrachtMaker);
        }

        // DELETE: api/OpdrachtMaker/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OpdrachtMaker>> DeleteOpdrachtMaker(long id)
        {
            var opdrachtMaker = await _context.OpdrachtMakers.FindAsync(id);
            if (opdrachtMaker == null)
            {
                return NotFound();
            }

            _context.OpdrachtMakers.Remove(opdrachtMaker);
            await _context.SaveChangesAsync();

            return opdrachtMaker;
        }

        private bool OpdrachtMakerExists(long id)
        {
            return _context.OpdrachtMakers.Any(e => e.Id == id);
        }
    }
}