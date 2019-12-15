using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class BedrijfTag
    {
        [Key]
        public long Id { get; set; }
        public long BedrijfId { get; set; }
        public long TagId { get; set; }
        public Bedrijf Bedrijf { get; set; }
        public Tag Tag { get; set; }
    }
}
