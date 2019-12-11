using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class Maker
    {
        [Key]
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public string Biografie { get; set; }
        public string LinkedInLink { get; set; }
        public int Ervaring { get; set; }
        public string GsmNr { get; set; }
        public string CV { get; set; }
        public string Foto { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<OpdrachtMaker> OpdrachtMakers { get; set; }

    }
}
