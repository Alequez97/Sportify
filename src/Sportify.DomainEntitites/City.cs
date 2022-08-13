using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    /// <summary>
    /// A class that represents City model
    /// </summary>
    public class City : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
