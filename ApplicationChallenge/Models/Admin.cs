using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class Admin
    {
        [Key]
        public long Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public UserLogin UserLogin { get; set; }
    }
}
