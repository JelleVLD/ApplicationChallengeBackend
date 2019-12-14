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
using System.Net;
using System.Security.Cryptography;
using ApplicationChallenge.Models.Dto;
using System.Security.Claims;
using System.Net.Mail;

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
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            if (gebruiker.Verified == false)
            {
                return Ok("Verify");
            }
            return Ok(gebruiker);
        }
        // GET: api/UserLogin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLogin>>> GetUserLogins()
        {
            return await _context.UserLogins.Include(m => m.Maker).Include(b => b.Bedrijf).Include(a => a.Admin).ToListAsync();
        }

        // GET: api/UserLogin/{userTypeId}
        [HttpGet("{userTypeId}")]
        public async Task<ActionResult<IEnumerable<UserLogin>>> GetUserLoginsWhereUserTypeId(int userTypeId)
        {
            return await _context.UserLogins.Include(m => m.Maker).Include(b => b.Bedrijf).Include(a => a.Admin).Where(u => u.UserTypeId == userTypeId).ToListAsync();
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
            string[] tags = data.tags;

            userLogin.UserTypeId = 2;
            userLogin.Verified = false;

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

            if (tags != null)
            {
                foreach(string tag in tags)
                {
                    Tag tagObject = await _context.Tags.Where(x => x.Naam == tag).SingleOrDefaultAsync();
                    if (tagObject == null)
                    {
                        tagObject = new Tag();
                        tagObject.Naam = tag;

                        _context.Tags.Add(tagObject);
                    }

                    MakerTag makertag = new MakerTag();
                    makertag.MakerId = maker.Id;
                    makertag.Interest = 1;
                    makertag.TagId = tagObject.Id;
                    makertag.SelfSet = true;
                    _context.MakerTags.Add(makertag);
                }
            }

            userLogin.MakerId = maker.Id;
            userLogin.Password = HashPassword(userLogin.Password);

            _context.UserLogins.Add(userLogin);
            await _context.SaveChangesAsync();

            MailAddress to = new MailAddress(userLogin.Email);
            MailAddress from = new MailAddress("donotreply@centtask.be");

            MailMessage message = new MailMessage(from, to);
            message.Subject = "Bevestig je account bij Centask";
            message.Body = "" +
                "<h1>Welkom bij Centask!</h1>" +
                "<p>Gelieve je account te bevestigen: " +
                "<a href='url'>http://localhost:4200/verifyUser?userLoginId=" + userLogin.Id + "</a>";

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

            return CreatedAtAction("GetUserLogin", new { id = userLogin.Id }, userLogin);
        }

        // POST: api/UserLogin
        [HttpPost("AddLoginBedrijf")]
        public async Task<ActionResult<UserLogin>> AddLoginBedrijf(BedrijfWithLogin data)
        {
            UserLogin userLogin = data.userlogin;
            Bedrijf bedrijf = data.bedrijf;

            userLogin.UserTypeId = 3;
            userLogin.Verified = false;

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

            MailAddress to = new MailAddress(userLogin.Email);
            MailAddress from = new MailAddress("donotreply@centtask.be");

            MailMessage message = new MailMessage(from, to);
            message.Subject = "Bevestig je account bij Centask";
            message.Body = "" +
                "<h1>Welkom bij Centask!</h1>" +
                "<p>Gelieve je account te bevestigen: " +
                "<a href='url'>http://localhost:4200/verifyUser?userLoginId=" + userLogin.Id + "</a>";

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


        [HttpPost("verifyUser")]
        public async Task<ActionResult<verifyUser>> VerifyUser(verifyUser verifyUser)
        {
            var id = verifyUser.Id;

            var userLoginOld = await _context.UserLogins.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (userLoginOld == null)
            {
                return Ok("Verkeerd");
            }


            UserLogin userLogin = new UserLogin();

            userLogin.Id = userLoginOld.Id;
            userLogin.Username = userLoginOld.Username;
            userLogin.Email = userLoginOld.Email;
            userLogin.MakerId = userLoginOld.MakerId;
            userLogin.BedrijfId = userLoginOld.BedrijfId;
            userLogin.UserTypeId = userLoginOld.UserTypeId;
            userLogin.AdminId = userLoginOld.AdminId;
            userLogin.Password = userLoginOld.Password;

            userLogin.Verified = true;

            _context.Entry(userLoginOld).State = EntityState.Detached;

            _context.Entry(userLogin).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("resetPassword")]
        public void restPassword(resetPassword emailadres)
        {
            var email = emailadres.email;
            var userLoginOld = _context.UserLogins.Where(x => x.Email == email).SingleOrDefault();

            if (userLoginOld == null)
            {
                return;
            }



            UserLogin userLogin = new UserLogin();

            userLogin.Id = userLoginOld.Id;
            userLogin.Username = userLoginOld.Username;
            userLogin.Email = userLoginOld.Email;
            userLogin.MakerId = userLoginOld.MakerId;
            userLogin.BedrijfId = userLoginOld.BedrijfId;
            userLogin.UserTypeId = userLoginOld.UserTypeId;
            userLogin.AdminId = userLoginOld.AdminId;

            var password = CreateRandomPassword(8);

            userLogin.Password = HashPassword(password);

            _context.Entry(userLoginOld).State = EntityState.Detached;

            _context.Entry(userLogin).State = EntityState.Modified;

            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress("donotreply@centtask.be");

            MailMessage message = new MailMessage(from, to);
            message.Subject = "Wachtwoord opnieuw ingesteld";
            message.Body = "" +

                "<h1>Wachtwoord opniew ingesteld</h1>" +
                "<p>U nieuw wachtwoord is '" +
                password +
                "'.";
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
            _context.SaveChangesAsync();
        }


        // HELPERS

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

        private static string CreateRandomPassword(int length = 15)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }
    }
}
