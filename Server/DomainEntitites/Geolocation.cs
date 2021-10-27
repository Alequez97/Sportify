using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    public class Geolocation : EntityBase
    {
        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }
    }
}
