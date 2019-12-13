﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationChallenge.Models;
using ApplicationChallenge.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Security.Cryptography;
using ApplicationChallenge.Models.Dto;
using System.Security.Claims;

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
        [Authorize]
        [HttpGet("loginInfo")]
        public async Task<ActionResult<LoginInfo>> GetLoginInfo()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "UserLoginId").Value;

            if (id == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogins.Where(x => x.Id == Convert.ToInt64(id)).SingleOrDefaultAsync();

            if (userLogin == null)
            {
                return NotFound();
            }

            LoginInfo loginInfo = new LoginInfo(userLogin.Username, userLogin.Email);

            return loginInfo;
        }

        // GET: api/UserLogin/userInfo
        [Authorize]
        [HttpGet("userInfo")]
        public async Task<ActionResult<UserLogin>> GetUserInfo()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "UserLoginId").Value;

            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserLogins
                .Where(x => x.Id == Convert.ToInt64(id))
                .Include(x => x.Maker)
                .Include(x => x.Bedrijf)
                .Include(x => x.Admin)
                .Include(x => x.UserType)
                .SingleOrDefaultAsync();

            if (userInfo == null)
            {
                return NotFound();
            }

            return userInfo;
        }

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

        [HttpPut("changePassword/{id}")]
        public async Task<IActionResult> ChangePassword(long id, UserLogin userLogin)
        {
            var userLoginId = User.Claims.FirstOrDefault(c => c.Type == "UserLoginId").Value;

            if (userLoginId == null)
            {
                return NotFound();
            }

            var userLoginOld = _context.UserLogins.Find(Convert.ToInt64(userLoginId));

            userLogin.Id = userLoginOld.Id;
            userLogin.Username = userLoginOld.Username;
            userLogin.Email = userLoginOld.Email;
            userLogin.MakerId = userLoginOld.MakerId;
            userLogin.BedrijfId = userLoginOld.BedrijfId;
            userLogin.UserTypeId = userLoginOld.UserTypeId;
            userLogin.AdminId = userLoginOld.AdminId;

            userLogin.Password = HashPassword(userLogin.Password);

            _context.Entry(userLoginOld).State = EntityState.Detached;

            _context.Entry(userLogin).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();

            //if (_context.UserLogins.Where(x => x.Username == userLogin.Username).SingleOrDefaultAsync() != null)
            //{
            //    return Ok("Username");
            //}

            //if (_context.UserLogins.Where(x => x.Email == userLogin.Email).SingleOrDefaultAsync() != null)
            //{
            //    return Ok("Username");
            //}



        }

        [HttpPut("changeUserInfo/{id}")]
        public async Task<IActionResult> ChangeUserInfo(long id, UserLogin userLogin)
        {
            var userLoginId = User.Claims.FirstOrDefault(c => c.Type == "UserLoginId").Value;

            if (userLoginId == null)
            {
                return NotFound();
            }

            var userCheck = _context.UserLogins.Where(x => x.Username == userLogin.Username).SingleOrDefault();
            if (userCheck != null)
            {
                if (userCheck.Id.ToString() != userLoginId)
                {
                    return Ok("Username");
                }
            }

            userCheck = _context.UserLogins.Where(x => x.Email == userLogin.Email).SingleOrDefault();
            if (userCheck != null)
            {
                if (userCheck.Id.ToString() != userLoginId)
                {
                    return Ok("Email");
                }
            }

            var userLoginOld = _context.UserLogins.Find(Convert.ToInt64(userLoginId));

            userLogin.Id = userLoginOld.Id;
            userLogin.Password = userLoginOld.Password;
            userLogin.MakerId = userLoginOld.MakerId;
            userLogin.BedrijfId = userLoginOld.BedrijfId;
            userLogin.UserTypeId = userLoginOld.UserTypeId;
            userLogin.AdminId = userLoginOld.AdminId;

            _context.Entry(userLoginOld).State = EntityState.Detached;

            _context.Entry(userLogin).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
                       
        }

        // POST: api/UserLogin
        [HttpPost]
        public async Task<ActionResult<UserLogin>> PostUserLogin(UserLogin userLogin)
        {
            _context.UserLogins.Add(userLogin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserLogin", new { id = userLogin.Id }, userLogin);
        }


        // POST: api/UserLogin
        [HttpPost("AddLoginMaker")]
        public async Task<ActionResult<UserLogin>> AddLoginMaker(MakerWithLogin data)
        {
            UserLogin userLogin = data.userlogin;
            Maker maker = data.maker;

            userLogin.UserTypeId = 2;

            if (_context.UserLogins.Where(x => x.Email == userLogin.Email).SingleOrDefault() != null)
            {
                return Ok("Email");
            }

            if (_context.UserLogins.Where(x => x.Username == userLogin.Username).SingleOrDefault() != null)
            {
                return Ok("Username");
            }

            _context.Makers.Add(maker);
            await _context.SaveChangesAsync();

            userLogin.MakerId = maker.Id;
            userLogin.Password = HashPassword(userLogin.Password);

            _context.UserLogins.Add(userLogin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserLogin", new { id = userLogin.Id }, userLogin);
        }

        // POST: api/UserLogin
        [HttpPost("AddLoginBedrijf")]
        public async Task<ActionResult<UserLogin>> AddLoginBedrijf(BedrijfWithLogin data)
        {
            UserLogin userLogin = data.userlogin;
            Bedrijf bedrijf = data.bedrijf;

            userLogin.UserTypeId = 3;

            if (_context.UserLogins.Where(x => x.Email == userLogin.Email).SingleOrDefault() != null)
            {
                return Ok("Email");
            }

            if (_context.UserLogins.Where(x => x.Username == userLogin.Username).SingleOrDefault() != null)
            {
                return Ok("Username");
            }

            if (_context.Bedrijven.Where(x => x.Naam == bedrijf.Naam).SingleOrDefault() != null)
            {
                return Ok("Name");
            }

            _context.Bedrijven.Add(bedrijf);
            await _context.SaveChangesAsync();

            userLogin.BedrijfId = bedrijf.Id;
            userLogin.Password = HashPassword(userLogin.Password);

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

        string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 2000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string PasswordHash = Convert.ToBase64String(hashBytes);

            return PasswordHash;
        }
    }
}
