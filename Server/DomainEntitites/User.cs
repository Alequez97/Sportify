using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DomainEntities
{
    /// <summary>
    /// Class that represents User model
    /// </summary>
    public class User : IdentityUser
    {   
        public List<Event> Events { get; set; }
    }
}
