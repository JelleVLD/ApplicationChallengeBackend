using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationChallenge.Models;
using ApplicationChallenge.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public IUserService _userService;

        public UserLoginController(ApplicationContext context, IUserService UserService)
        {
            _context = context;
            _userService = UserService;
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserLogin userParam)
        {
            var gebruiker = _userService.Authenticate(userParam.Username, userParam.Password);
            if (gebruiker == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(gebruiker);
        }
        // GET: api/UserLogin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLogin>>> GetUserLogins()
        {
            return await _context.UserLogins.ToListAsync();
        }

        // GET: api/UserLogin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLogin>> GetUserLogin(long id)
        {
            var userLogin = await _context.UserLogins.FindAsync(id);

            if (userLogin == null)
            {
                return NotFound();
            }

            return userLogin;
        }

        // PUT: api/UserLogin/5
        //hier passen we de username mee aan, we zullen controleren dat we de username geen 2 keer wegschrijven
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserLogin(long id, UserLogin userLogin)
        {

            var userBestaat = _context.UserLogins.Where(u => u.Username == userLogin.Username);


            long userID = 0;

            foreach (var u in userBestaat)
            {
                userID = u.Id;
            }

            if (userBestaat.Count() < 1 || userID == id)
            {
                var userOld = _context.UserLogins.Find(id);

                userLogin.Password = userOld.Password;
                userLogin.MakerId = userOld.MakerId;
                userLogin.BedrijfId = userOld.BedrijfId;
                userLogin.AdminId = userOld.AdminId;
                userLogin.UserTypeId = userOld.UserTypeId;
                userLogin.Id = id;

                _context.Entry(userOld).State = EntityState.Detached;

                _context.Entry(userLogin).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                //try
                //{

                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!UserLoginExists(id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}

                return Ok(userLogin);

            }
            else
            {
                return NoContent();
            }

            
        }

        // POST: api/UserLogin
        [HttpPost]
        public async Task<ActionResult<UserLogin>> PostUserLogin(UserLogin userLogin)
        {
            _context.UserLogins.Add(userLogin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserLogin", new { id = userLogin.Id }, userLogin);
        }

        // DELETE: api/UserLogin/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserLogin>> DeleteUserLogin(long id)
        {
            var userLogin = await _context.UserLogins.FindAsync(id);
            if (userLogin == null)
            {
                return NotFound();
            }

            _context.UserLogins.Remove(userLogin);
            await _context.SaveChangesAsync();

            return userLogin;
        }

        private bool UserLoginExists(long id)
        {
            return _context.UserLogins.Any(e => e.Id == id);
        }
    }
}
