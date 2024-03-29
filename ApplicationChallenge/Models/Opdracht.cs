﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class Opdracht
    {
        [Key]
        public long Id { get; set; }
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public string Straat { get; set; }
        public string StraatNr { get; set; }
        public string WoonPlaats { get; set; }
        public string Postcode { get; set; }
        public long? BedrijfId { get; set; }
        public bool open { get; set; }
        public bool klaar { get; set; }
        public ICollection<Maker> Makers { get; set; }
        public ICollection<OpdrachtMaker> OpdrachtMakers { get; set; }
        public ICollection<OpdrachtTag> Tags { get; set; }
        public Bedrijf Bedrijf { get; set; }

        [NotMapped]
        public double Interest { get; set; }
        

    }
}
