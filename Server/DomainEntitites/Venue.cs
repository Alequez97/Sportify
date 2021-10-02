using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    public class Venue : EntityBase
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }
    }
}
