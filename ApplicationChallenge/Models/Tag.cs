using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class Tag
    {
        [Key]
        public long Id { get; set; }
        public string Naam { get; set; }
    }
}
