using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    /// <summary>
    /// Class that represents Event model
    /// </summary>
    public class Event : EntityBase
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public EventCategory Category { get; set; }

        [Required]
        [MaxLength(50)]
        public string BriefDesc { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        [Required]
        public DateTime TimeOfTheEvent { get; set; }

        [Required]
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public List<EventUser> EventUsers { get; set; }
    }
}