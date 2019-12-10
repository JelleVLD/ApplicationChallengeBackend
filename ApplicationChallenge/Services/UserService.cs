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
        public UserLogin Authenticate(string username, string password)
        {
            var user = _apiContext
                .UserLogins
                .SingleOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
                return null;

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
