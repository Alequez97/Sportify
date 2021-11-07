using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DomainEntities
{
    /// <summary>
    /// Class that represents User model
    /// </summary>
    public class User : IdentityUser<int>
    {   
        public List<EventUser> EventUsers { get; set; }
    }

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }
}