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
    public class OpdrachtController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public OpdrachtController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Opdracht
        [HttpGet]
        [Permission("Opdracht.OnGet")]
        public async Task<ActionResult<IEnumerable<Opdracht>>> GetOpdrachten()

        {
            var opdrachten = await _context.Opdrachten.Include(o => o.Bedrijf).Include(x => x.Tags).ThenInclude(y => y.Tag).ToListAsync();
            var userId = User.Claims.FirstOrDefault(c => c.Type == "GebruikerId").Value;
            var userTags = await _context.MakerTags.Where(x => x.MakerId == Convert.ToInt64(userId)).ToListAsync();

            // ADDING INTEREST AMOUNT TO EVERY TASK
            foreach (var opdracht in opdrachten)
            {
                double interest = 0;
                foreach (var tag in opdracht.Tags)
                {
                    foreach (var userTag in userTags)
                    {
                        if (tag.TagId == userTag.TagId)
                        {
                            interest = interest + userTag.Interest;
                        }
                    }
                }
                opdracht.Interest = interest;
            }

            return opdrachten;
        }

        // GET: api/Opdracht
        [HttpGet("open")]
        [Permission("Opdracht.OnGet")]
        public async Task<ActionResult<IEnumerable<Opdracht>>> GetOpdrachtenOpen()
        {
            var opdrachten = await _context.Opdrachten.Include(o => o.Bedrijf).Include(x => x.Tags).ThenInclude(y => y.Tag).Where(o => o.open == true).ToListAsync();
            var userId = User.Claims.FirstOrDefault(c => c.Type == "GebruikerId").Value;
            var userTags = await _context.MakerTags.Where(x => x.MakerId == Convert.ToInt64(userId)).ToListAsync();

            // ADDING INTEREST AMOUNT TO EVERY TASK
            foreach (var opdracht in opdrachten)
            {
                double interest = 0;
                foreach (var tag in opdracht.Tags)
                {
                    foreach (var userTag in userTags)
                    {
                        if (tag.TagId == userTag.TagId)
                        {
                            interest = interest + userTag.Interest;
                        }
                    }
                }
                opdracht.Interest = interest;
            }

            return opdrachten;
        }

        //GET: api/Opdracht
        [Permission("Opdracht.OnGet")]
        [HttpGet("searchOpen/{title}")]
        public async Task<ActionResult<IEnumerable<Opdracht>>> GetOpdrachtenByTitleAndTagsOnlyOpen(string title)

        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "GebruikerId").Value;

            // ADDING INTEREST TO TAG IF FOUND
            var tag = _context.Tags.Where(x => x.Naam == title.ToLower()).SingleOrDefault();
            if (tag != null)
            {
                var makerTag = _context.MakerTags.Where(x => (x.TagId == tag.Id) && (x.MakerId == Convert.ToInt64(userId))).SingleOrDefault();
                if (makerTag != null)
                {
                    if (makerTag.SelfSet == false)
                    {
                        _context.Entry(makerTag).State = EntityState.Modified;
                        makerTag.Interest = (makerTag.Interest + (makerTag.Interest / 10 * 3));
                    }
                }
                else
                {
                    makerTag = new MakerTag();
                    makerTag.MakerId = Convert.ToInt64(userId);
                    makerTag.TagId = tag.Id;
                    makerTag.Interest = 0.3;
                    makerTag.SelfSet = false;
                    _context.Add(makerTag);
                }
                await _context.SaveChangesAsync();
            }
            var opdrachten = await _context.Opdrachten.Include(o => o.Bedrijf).Where(o => (o.Titel.ToLower().Contains(title.ToLower()) || o.Bedrijf.Naam.ToLower().Contains(title.ToLower())) && o.open == true).Include(x => x.Tags).ThenInclude(y => y.Tag).ToListAsync();
            var opdrachtenTags = await _context.Opdrachten.Include(o => o.Bedrijf).Include(x => x.Tags).ThenInclude(y => y.Tag).Where(o => o.open == true).ToListAsync();
            var userTags = await _context.MakerTags.Where(x => x.MakerId == Convert.ToInt64(userId)).ToListAsync();

            foreach (var opdracht in opdrachtenTags)
            {
                foreach (var tagY in opdracht.Tags)
                {
                    if (tagY.Tag != null)
                    {
                        if (tagY.Tag.Naam.ToLower().Contains(title.ToLower()) && !opdrachten.Contains(opdracht))
                        {
                            opdrachten.Add(opdracht);
                        }
                    }
                }
            }


            // ADDING INTEREST AMOUNT TO EVERY TASK
            foreach (var opdracht in opdrachten)
            {
                double interest = 0;
                foreach (var tagY in opdracht.Tags)
                {
                    foreach (var userTag in userTags)
                    {
                        if (tagY.TagId == userTag.TagId)
                        {
                            interest = interest + userTag.Interest;
                        }
                    }
                }
                opdracht.Interest = interest;
            }

            if (opdrachten == null)
            {
                return NotFound();
            }

            return opdrachten;
        }

        //GET: api/Opdracht
        [Permission("Opdracht.OnGet")]
        [HttpGet("search/{title}")]
        public async Task<ActionResult<IEnumerable<Opdracht>>> GetOpdrachtenByTitleAndTags(string title)

        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "GebruikerId").Value;

            // ADDING INTEREST TO TAG IF FOUND
            var tag = _context.Tags.Where(x => x.Naam == title.ToLower()).SingleOrDefault();
            if (tag != null)
            {
                var makerTag = _context.MakerTags.Where(x => (x.TagId == tag.Id) && (x.MakerId == Convert.ToInt64(userId))).SingleOrDefault();
                if (makerTag != null)
                {
                    if (makerTag.SelfSet == false)
                    {
                        _context.Entry(makerTag).State = EntityState.Modified;
                        makerTag.Interest = (makerTag.Interest + (makerTag.Interest / 10 * 3));
                    }
                } else
                {
                    makerTag = new MakerTag();
                    makerTag.MakerId = Convert.ToInt64(userId);
                    makerTag.TagId = tag.Id;
                    makerTag.Interest = 0.3;
                    makerTag.SelfSet = false;
                    _context.Add(makerTag);
                }
                await _context.SaveChangesAsync();
            }
            var opdrachten = await _context.Opdrachten.Include(o => o.Bedrijf).Where(o => o.Titel.ToLower().Contains(title.ToLower()) || o.Bedrijf.Naam.ToLower().Contains(title.ToLower())).Include(x => x.Tags).ThenInclude(y => y.Tag).ToListAsync();
            var opdrachtenTags = await _context.Opdrachten.Include(o => o.Bedrijf).Include(x => x.Tags).ThenInclude(y => y.Tag).ToListAsync();
            var userTags = await _context.MakerTags.Where(x => x.MakerId == Convert.ToInt64(userId)).ToListAsync();

            foreach (var opdracht in opdrachtenTags)
            {
                foreach (var tagY in opdracht.Tags)
                {
                    if (tagY.Tag != null)
                    {
                        if (tagY.Tag.Naam.ToLower().Contains(title.ToLower()) && !opdrachten.Contains(opdracht))
                        {
                            opdrachten.Add(opdracht);
                        }
                    }
                }
            }


            // ADDING INTEREST AMOUNT TO EVERY TASK
            foreach (var opdracht in opdrachten)
            {
                double interest = 0;
                foreach (var tagY in opdracht.Tags)
                {
                    foreach (var userTag in userTags)
                    {
                        if (tagY.TagId == userTag.TagId)
                        {
                            interest = interest + userTag.Interest;
                        }
                    }
                }
                opdracht.Interest = interest;
            }

            if (opdrachten == null)
            {
                return NotFound();
            }

            return opdrachten;
        }

        // GET: api/Opdracht/5
        [Permission("Opdracht.OnGet")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Opdracht>> GetOpdracht(long id)
        {
            var opdracht = await _context.Opdrachten.Include(o => o.Bedrijf).Include(o => o.OpdrachtMakers).Include(o => o.Makers).FirstOrDefaultAsync(o => o.Id == id);
            //var opdracht = await _context.Opdrachten.FindAsync(id);

            if (opdracht == null)
            {
                return NotFound();
            }

            return opdracht;
        }

        // PUT: api/Opdracht/5
        [Permission("Opdracht.OnPutID")]
        [HttpPut("{id}")]
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
        // PUT: api/Opdracht/5
        [HttpPut("sluitopdracht/{id}")]
        [Permission("Opdracht.OnPutID")]
        public async Task<IActionResult> SluitOpdracht(long id,OpdrachtMaker opdrachtmaker)
        {
            var opdracht = await _context.Opdrachten.FindAsync(opdrachtmaker.OpdrachtId);
            var userLogin = await _context.UserLogins.FindAsync(opdrachtmaker.MakerId);
            if (id != opdracht.Id)
            {
                return BadRequest();
            }
            opdracht.klaar = true;
            _context.Entry(opdracht).State = EntityState.Modified;
            MailAddress to = new MailAddress(userLogin.Email);
            MailAddress from = new MailAddress("donotreply@centtask.be");

            MailMessage message = new MailMessage(from, to);
            message.Subject = "Betreffende " +opdracht.Titel;
            message.Body = "" +
                "<h1>Gefeliciteerd!</h1>" +
                "<p>U heeft de opdracht: " + opdracht.Titel + " gewonnen!</p>" +
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
