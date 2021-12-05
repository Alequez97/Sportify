using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    /// <summary>
    /// Class that represents Country model
    /// </summary>
    public class Country : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(3)]
        public string Iso3 { get; set; }
        
        [MaxLength(2)]
        public string Iso2 { get; set; }

        public List<City> Cities { get; set; }
    }
}
