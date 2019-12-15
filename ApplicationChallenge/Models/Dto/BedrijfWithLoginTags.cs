using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models.Dto
{
    public class BedrijfWithLoginTags
    {
        public Bedrijf bedrijf;
        public UserLogin userlogin;
        public string[] tags;
    }
}
