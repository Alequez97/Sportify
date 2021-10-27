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
        [Required]
        public string Title { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        [MaxLength(50)]
        public string BriefDesc { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public Venue Venue { get; set; }

        [Required]
        public DateTime TimeOfTheEvent { get; set; }

        [Required]
        public User Creator { get; set; }

        public List<User> Contributors { get; set; }
    }
}
