﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocaching.Models
{
    public class FoundGeocache
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int GeocasheId { get; set; }
        public Geocashe Geocashe { get; set; }
    }
}
