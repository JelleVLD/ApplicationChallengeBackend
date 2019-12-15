using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationChallenge.Models;
using ApplicationChallenge.Attributes;
using System.Net.Mail;
using System.Net;

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
            return await _context.OpdrachtMakers.Include(o => o.Opdracht).Include(o => o.Opdracht.Bedrijf).ToListAsync();
        }

        // GET: api/OpdrachtMaker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OpdrachtMaker>>> GetOpdrachtMaker(long id)
        {
            var opdrachtMaker = await _context.OpdrachtMakers.Include(o => o.Opdracht).Include(o => o.Opdracht.Bedrijf).Where(o => o.MakerId == id).ToListAsync();

            if (opdrachtMaker == null)
            {
                return NotFound();
            }

            return opdrachtMaker;
        }
        [HttpGet]
        [Route("getid/{opdrachtId}")]
        [Permission("OpdrachtMaker.OnGetId")]
        public async Task<ActionResult<IEnumerable<OpdrachtMaker>>> GetOpdrachtMakers(long opdrachtId)
        {
            var opdrachtMakers = await _context.OpdrachtMakers.Include(m => m.Maker).Where(o => o.OpdrachtId == opdrachtId).ToListAsync();

            if (opdrachtMakers == null)
            {
                return NotFound();
            }

            return opdrachtMakers;
        }

        // PUT: api/OpdrachtMaker/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOpdrachtMaker(long id, OpdrachtMaker opdrachtMaker)
        {
            if (id != opdrachtMaker.Id)
            {
                return BadRequest();
            }
            var newOpdrachtMaker = await _context.OpdrachtMakers.Include(o => o.Opdracht).Where(o => o.Id == opdrachtMaker.Id).FirstAsync();

            var userLogin = await _context.UserLogins.Where(u => u.MakerId == opdrachtMaker.MakerId).FirstAsync();
            MailAddress to = new MailAddress(userLogin.Email);
            MailAddress from = new MailAddress("donotreply@centtask.be");

            MailMessage message = new MailMessage(from, to);
            message.Subject = "Betreffende " + newOpdrachtMaker.Opdracht.Titel;
            message.Body = "" +
                "<h1>Gefeliciteerd!</h1>" +
                "<p>U bent  geselecteerd voor de opdracht: " + newOpdrachtMaker.Opdracht.Titel + ", en kan er nu aan beginnen!</p>" +
                "<p>Met vriendelijke groeten, het Centtask team;</p>";

            message.IsBodyHtml = true;

            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("9533f03187ee7c", "3058369c7f2f3b"),
                EnableSsl = true
            };

            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            newOpdrachtMaker.Geaccepteerd = true;
            _context.Entry(newOpdrachtMaker).State = EntityState.Modified;
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

        //nakijken of er al is gestemd voor een opdracht
        // PUT: api/OpdrachtMaker/5
        [HttpPut("get{id}")]
        public async Task<ActionResult<IList<OpdrachtMaker>>> PutgetOpdrachtMaker(long id, OpdrachtMaker opdrachtMaker)
        {
            var vopdrachtmaker = await _context.OpdrachtMakers.Where(o => o.OpdrachtId == opdrachtMaker.OpdrachtId).Where(o => o.MakerId == opdrachtMaker.MakerId).ToListAsync();


            int count = 0;

            var opdrachtmaker = new List<OpdrachtMaker>();

            foreach (var o in vopdrachtmaker)
            {
                opdrachtmaker.Add(new OpdrachtMaker { Id = o.Id });
                count++;
            }

            if (count > 0)
            {
                return opdrachtmaker;
            }
            else
            {
                return NoContent();
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
        public async Task<ActionResult<OpdrachtMaker>> DeleteOpdrachtMaker(long id) {


            var opdrachtMaker = await _context.OpdrachtMakers.Include(m => m.Maker).Include(o=>o.Opdracht).Where(i => i.Id == id).FirstOrDefaultAsync();
            if (opdrachtMaker == null)
            {
                return NotFound();
            }
            var userLogin = await _context.UserLogins.Where(u => u.MakerId == opdrachtMaker.MakerId).FirstAsync();
            MailAddress to = new MailAddress(userLogin.Email);
            MailAddress from = new MailAddress("donotreply@centtask.be");

            MailMessage message = new MailMessage(from, to);
            message.Subject = "Betreffende " + opdrachtMaker.Opdracht.Titel;
            message.Body = "" +
                "<h1>Onze excuses!</h1>" +
                "<p>U bent jammergenoeg niet geselecteerd voor de opdracht: " + opdrachtMaker.Opdracht.Titel + "</p>" +
                "<p>Met vriendelijke groeten, het Centtask team;</p>";

            message.IsBodyHtml = true;

            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("9533f03187ee7c", "3058369c7f2f3b"),
                EnableSsl = true
            };

            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            _context.OpdrachtMakers.Remove(opdrachtMaker);
            await _context.SaveChangesAsync();

            return opdrachtMaker;
        }

        // DELETE: api/OpdrachtMaker/makerid/5
        [HttpDelete("makerId/{makerId}")]
        public async Task<ActionResult<IEnumerable<OpdrachtMaker>>> DeleteOpdrachtMakerWhereMakerId(int makerId)
        {
            var opdrachtMakers = await _context.OpdrachtMakers.Where(m => m.MakerId == makerId).ToListAsync();
            if (opdrachtMakers == null)
            {
                return NotFound();
            }
            foreach (var opdrachtMaker in opdrachtMakers)
            {
                _context.OpdrachtMakers.Remove(opdrachtMaker);
            }
            await _context.SaveChangesAsync();

            return opdrachtMakers;
        }

        private bool OpdrachtMakerExists(long id)
        {
            return _context.OpdrachtMakers.Any(e => e.Id == id);
        }
    }
}
