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
    public class Register : EndpointBaseAsync
        .WithRequest<RegisterRequest>
        .WithActionResult<RegisterResponse>
    {
        private readonly UserManager<User> _userManager;

        public Register(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("/api/accounts/register")]
        [SwaggerOperation(Tags = new[] { "Accounts" })]
        public override async Task<ActionResult<RegisterResponse>> HandleAsync([FromBody]RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var userExists = await _userManager.FindByNameAsync(request.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponse { Status = "Error", Message = "User already exists!" });

            var user = new User()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new RegisterResponse { Status = "Success", Message = "User created successfully!" });
        }
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class RegisterResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
