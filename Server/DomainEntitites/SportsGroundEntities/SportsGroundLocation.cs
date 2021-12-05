using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainEntities.SportsGroundEntities
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

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required]
        public int TypeId { get; set; }
        public SportsGroundType Type { get; set; }

        [Required]
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public List<SportsGroundImage> Images { get; set; }
    }
}
