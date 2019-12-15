using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class MakerTag
    {
        [Key]
        public long Id { get; set; }
        public long MakerId { get; set; }
        public long TagId { get; set; }
        public Tag Tag { get; set; }
        public bool SelfSet { get; set; }
        public double Interest { get; set; }
    }
}
