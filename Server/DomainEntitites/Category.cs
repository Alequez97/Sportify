using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    public class Category : EntityBase
    {
        [Required]
        public string Name { get; set; }
    }
}
