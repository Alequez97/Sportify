using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SportifyWebApi.Endpoints.Accounts
{
    public class GetAuthenticatedUserInfo : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<GetAuthenticatedUserInfoResponse>
    {
        [Authorize]
        [HttpGet("api/accounts/info")]
        public override async Task<ActionResult<GetAuthenticatedUserInfoResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var info = new GetAuthenticatedUserInfoResponse()
            {
                Id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Username = User.FindFirst(ClaimTypes.Name).Value,
                IsAdmin = User.FindAll(ClaimTypes.Role).Any(role => role.Value == UserRoles.Admin)
            };

            return Ok(info);
        }
    }

    public class GetAuthenticatedUserInfoResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
    }
}