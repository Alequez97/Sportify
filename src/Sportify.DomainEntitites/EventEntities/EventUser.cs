namespace DomainEntities.EventEntities
{
    /// <summary>
    /// Class that represents many-to-many relationship between Event and User
    /// </summary>
    public class EventUser : EntityBase
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public Event Event { get; set; }
        public User User { get; set; }
        public bool IsGoing { get; set; }
    }
}
