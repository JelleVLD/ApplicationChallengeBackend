using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class OpdrachtTag
    {

        [Key]
        public long Id { get; set; }
        public long? OpdrachtId { get; set; }
        public long? TagId { get; set; }
        public Opdracht Opdracht { get; set; }
        public Tag Tag { get; set; }
    }
}
