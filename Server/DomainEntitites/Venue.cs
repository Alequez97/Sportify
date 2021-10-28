using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    public class Venue : EntityBase
    {
        public Country Country { get; set; }

        public City City { get; set; }

        public string Address { get; set; }
    }
}
