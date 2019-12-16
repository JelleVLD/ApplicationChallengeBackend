using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models.Dto
{
    public class OpdrachtTags
    {
        public Opdracht opdracht { get; set; }
        public string[] tags;
    }
}
