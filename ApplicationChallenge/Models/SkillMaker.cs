using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class SkillMaker
    {
        [Key]
        public long Id { get; set; }
        public long SkillId { get; set; }
        public long MakerId { get; set; }
        public int ExpertisePercentage { get; set; }

        public Skill Skill { get; set; }
        public Maker Maker { get; set; }

    }
}
