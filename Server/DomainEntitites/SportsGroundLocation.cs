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

        public string District { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        [Required]
        public int TypeId { get; set; }
        public SportsGroundType Type { get; set; }

        public int GeolocationId { get; set; }
        public Geolocation Geolocation { get; set; }
    }
}
