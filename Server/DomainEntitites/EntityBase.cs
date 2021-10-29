using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    /// <summary>
    /// Base class with Id field
    /// </summary>
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
