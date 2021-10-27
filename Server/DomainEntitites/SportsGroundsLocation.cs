﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    class SportsGroundsLocation : EntityBase
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public Geolocation Geolocation { get; set; }
    }
}
