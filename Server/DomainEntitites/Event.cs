using System;
using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    /// <summary>
    /// Class that represents event model
    /// </summary>
    public class Event
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Venue Venue { get; set; }

        public DateTime TimeOfTheEvent { get; set; }

        //public User CreatorId { get; set; }

        //public List<User> Contributors { get; set; }
    }
}
