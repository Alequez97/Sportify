using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
