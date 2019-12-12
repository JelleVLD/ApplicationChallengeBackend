using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long UserTypeId { get; set; }
    }
}
