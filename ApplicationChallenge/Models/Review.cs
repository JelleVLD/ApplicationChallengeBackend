using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class Review
    {
        [Key]
        public long Id { get; set; }
        public long MakerId { get; set; }
        public long BedrijfId { get; set; }
        public int Score { get; set; }
        public string ReviewTekst { get; set; }
        public bool NaarBedrijf { get; set; }

        public Maker Maker { get; set; }
        public Bedrijf Bedrijf { get; set; }

    }
}
