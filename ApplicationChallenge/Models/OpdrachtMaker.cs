using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class OpdrachtMaker
    {
        [Key]
        public long Id { get; set; }
        public long MakerId { get; set; }
        public Maker Maker { get; set; }

        public long OpdrachtId { get; set; }
        public Opdracht Opdracht { get; set; }

    }
}
