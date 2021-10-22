using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    /// <summary>
    /// Class that represents event model
    /// </summary>
    public class Event : EntityBase
    {
        public string Title { get; set; }

        public Category Category { get; set; }

        public string BriefDesc { get; set; }

        public string Description { get; set; }

        public Venue Venue { get; set; }

        public DateTime TimeOfTheEvent { get; set; }

        public User CreatorId { get; set; }

        public List<User> Contributors { get; set; }
    }
}
