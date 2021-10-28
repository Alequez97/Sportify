using System;
using System.Collections.Generic;

namespace CountriesCitiesJsonModifier
{
    public class Country
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Iso3 { get; set; }
        public string Iso2 { get; set; }
        public List<City> Cities { get; set; }
    }
}