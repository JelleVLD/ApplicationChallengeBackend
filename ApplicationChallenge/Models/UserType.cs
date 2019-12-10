using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class UserType
    {
        [Key]
        public long Id { get; set; }
        public string Soort { get; set; }
        public ICollection<Permission> Permissions {get; set;}
    }
}
