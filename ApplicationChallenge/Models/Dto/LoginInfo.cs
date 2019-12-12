using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models.Dto
{
    public class LoginInfo
    {
        public string Username;
        public string Email;

        public LoginInfo(string username, string email)
        {
            Username = username;
            Email = email;
        }
    }
}
