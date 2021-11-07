using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    public class EventUser : EntityBase
    {
        public int EventId { get; set; }
        [Key]
        public Event Event { get; set; }
        public int UserId { get; set; }
        [Key]
        public User User { get; set; }
    }
}
