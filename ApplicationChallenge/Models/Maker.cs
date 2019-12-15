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
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Straat { get; set; }
        public string Nr { get; set; }
        public string Stad { get; set; }
        public string Postcode { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public string Biografie { get; set; }
        public string LinkedInLink { get; set; }
        public string Ervaring { get; set; }
        public string Foto { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<OpdrachtMaker> OpdrachtMakers { get; set; }
        public ICollection<MakerTag> Interesses { get; set; }

    }
}
