using System.Collections.Generic;

namespace DomainEntities
{
    public class Country : EntityBase
    {
        public string Name { get; set; }
        public string Iso3 { get; set; }
        public string Iso2 { get; set; }
        public List<City> Cities { get; set; }
    }
}
