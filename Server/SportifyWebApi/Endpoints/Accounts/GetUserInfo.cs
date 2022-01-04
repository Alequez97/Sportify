using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Accounts
{
    public class GetUserInfo : BaseAsyncEndpoint
        .WithRequest<GetUserInfoRequest>
        .WithResponse<GetUserInfoResponse>
    {
        private readonly SportifyDbContext _context;

        public GetUserInfo(SportifyDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("/api/accounts/user-info/{username}")]
        [SwaggerOperation(Tags = new[] { "Accounts" })]
        public override async Task<ActionResult<GetUserInfoResponse>> HandleAsync([FromRoute]GetUserInfoRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .Where(u => string.Equals(u.NormalizedUserName, request.UserName))
                .Select(u => new GetUserInfoResponse
                {
                    Username = u.UserName,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            return user != null ? Ok(user) : NotFound() ;
        }
    }

    public class GetUserInfoRequest
    {
        [Required]
        public string UserName { get; set; }
    }
    
    public class GetUserInfoResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
