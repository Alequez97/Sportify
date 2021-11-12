using System.ComponentModel.DataAnnotations;

namespace DomainEntities.EventEntities
{
    /// <summary>
    /// Class that represents Category model
    /// </summary>
    public class EventCategory : EntityBase
    {
        [Required]
        public string Name { get; set; }
    }
}
