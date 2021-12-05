using System.ComponentModel.DataAnnotations;

namespace DomainEntities.EventEntities
{
    /// <summary>
    /// Class that represents Venue model
    /// </summary>
    public class Venue : EntityBase
    {
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}