using System;
using System.Linq;
using System.Security.Claims;
using SportifyWebApi.Authentication;

namespace SportifyWebApi.Services
{
    public class AuthenticationService
    {
        public InfoModel GetAuthenticatedUserInfo(ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated 
                ? 
                new InfoModel()
                {
                    Id = Convert.ToInt32(user.FindFirst(ClaimTypes.NameIdentifier).Value),
                    Username = user.FindFirst(ClaimTypes.Name).Value,
                    IsAdmin = user.FindAll(ClaimTypes.Role).Any(role => role.Value == UserRoles.Admin)
                }
                :
                null;
        }
    }
}
