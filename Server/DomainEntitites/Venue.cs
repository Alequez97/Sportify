using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    class Venue
    {
        [Key]
        public int Id { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }
    }
}
