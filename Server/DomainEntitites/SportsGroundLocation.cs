using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    /// <summary>
    /// Class that represents SportsGroundsLocation model
    /// </summary>
    public class SportsGroundLocation : EntityBase
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public SportsGroundCategory Category { get; set; }

        public int GeolocationId { get; set; }
        public Geolocation Geolocation { get; set; }
    }
}
