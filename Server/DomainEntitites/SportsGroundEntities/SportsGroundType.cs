using System.ComponentModel.DataAnnotations;

namespace DomainEntities.SportsGroundEntities
{
    public class SportsGroundType : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}