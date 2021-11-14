using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DomainEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Accounts
{
    public class RegisterAdmin : BaseAsyncEndpoint
        .WithRequest<RegisterAdmimRequest>
        .WithResponse<RegisterAdminResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public RegisterAdmin(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("/api/accounts/register-admin")]
        [SwaggerOperation(Tags = new[] { "Accounts" })]
        public override async Task<ActionResult<RegisterAdminResponse>> HandleAsync([FromBody]RegisterAdmimRequest request, CancellationToken cancellationToken = default)
        {
            var userExists = await _userManager.FindByNameAsync(request.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterAdminResponse { Status = "Error", Message = "User already exists!" });

            var user = new User()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterAdminResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole<int>(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole<int>(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new RegisterAdminResponse { Status = "Success", Message = "User created successfully!" });
        }
    }

    public class RegisterAdmimRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class RegisterAdminResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
