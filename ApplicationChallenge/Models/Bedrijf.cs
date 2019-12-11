using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class Bedrijf
    {
        [Key]
        public long Id { get; set; }
        public string Naam { get; set; }
        public string Straat { get; set; }
<<<<<<< HEAD
        public string Nr { get; set; }
        public string Stad { get; set; }
        public int Postcode { get; set; }
=======
        public string StraatNr { get; set; }
        public string WoonPlaats { get; set; }
        public string Postcode { get; set; }
>>>>>>> aanpassingen bedrijf en opdrachtmodel + insert opdracht werkt
        public string Biografie { get; set; }
        public string Foto { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Opdracht> Opdrachten { get; set; }

    }
}
