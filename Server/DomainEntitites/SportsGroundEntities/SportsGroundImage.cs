using System.ComponentModel.DataAnnotations;

namespace DomainEntities.SportsGroundEntities
{
    public class SportsGroundImage : EntityBase
    {
        public string Path { get; set; }

        [Required]
        public int SportsGroundLocationId { get; set; }
        public SportsGroundLocation SportsGroundLocation { get; set; }
    }
}
