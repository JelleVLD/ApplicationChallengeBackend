using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class Opdracht
    {
        [Key]
        public long Id { get; set; }
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public string Locatie { get; set; }
        public long BedrijfId { get; set; }
        public ICollection<Maker> Makers { get; set; }
        public Bedrijf Bedrijf { get; set; }

    }
}
