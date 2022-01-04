using System.ComponentModel.DataAnnotations;

namespace DomainEntities.EventEntities
{
    /// <summary>
    /// Class that represents Category model
    /// </summary>
    /// 
    public class EventCategory : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
