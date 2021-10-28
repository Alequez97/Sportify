using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    /// <summary>
    /// Class that represents Venue model
    /// </summary>
    public class Venue : EntityBase
    {
        public Country Country { get; set; }

        public City City { get; set; }

        public string Address { get; set; }

        public int GeolocationId { get; set; }
        public Geolocation Geolocation { get; set; }
    }
}
