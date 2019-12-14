using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class UserLogin
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public long? MakerId { get; set; }
        public long? BedrijfId { get; set; }
        public long UserTypeId { get; set; }
        public long? AdminId { get; set; }
        [NotMapped]
        public string Token { get; set; }
        public Maker Maker { get; set; }
        public Bedrijf Bedrijf { get; set; }
        public Admin Admin { get; set; }
        public UserType UserType{get;set;}
        public bool Verified { get; set; }

    }
}
