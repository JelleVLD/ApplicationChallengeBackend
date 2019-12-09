using ApplicationChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Services
{
   public interface IUserService
    {
UserLogin Authenticate(string username, string password);
    
}
}
