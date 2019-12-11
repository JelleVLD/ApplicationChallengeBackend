using ApplicationChallenge.Helpers;
using ApplicationChallenge.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace ApplicationChallenge.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationContext _apiContext;
        public UserService(IOptions<AppSettings> appSettings, ApplicationContext apiContext)
        {
            _appSettings = appSettings.Value;
            _apiContext = apiContext;
        }

        private bool VerifyPassword(string password, string savedPasswordHash)
        {
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 2000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
        public UserLogin Authenticate(string username, string password)
        {
            var user = _apiContext
                .UserLogins
                .SingleOrDefault(x => x.Username == username);

            if (user == null)
            {
                user = _apiContext
                .UserLogins
                .SingleOrDefault(x => x.Email == username);
            }

            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(password, user.Password))
            {
                return null;
            }

            var userType = _apiContext.UserTypes.SingleOrDefault(x => x.Id == user.UserTypeId);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var gebruikerId="";
            if (user.MakerId.ToString() != "")
            {
                gebruikerId = user.MakerId.ToString();
            }
            else
            {
                if (user.BedrijfId.ToString() != "")
                {
                    gebruikerId= user.BedrijfId.ToString();
                }
                else
                {
                   gebruikerId= user.AdminId.ToString();
                }
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("UserLoginId", user.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim("Password", user.Password),
                new Claim(ClaimTypes.Role, userType.Soort),
                new Claim("GebruikerId", gebruikerId) }),


                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;
            return user;
        }

    }
}
