using System;
using System.Linq;
using System.Security.Claims;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Sportify.Api.Endpoints.Accounts;

public class GetAuthenticatedUserInfo : EndpointBaseSync
  .WithoutRequest
  .WithActionResult<GetAuthenticatedUserInfoResponse>
{
  [Authorize]
  [HttpGet("/api/accounts/auth-user-info")]
  [SwaggerOperation(Tags = new[] { "Accounts" })]
  public override ActionResult<GetAuthenticatedUserInfoResponse> Handle()
  {
    var info = new GetAuthenticatedUserInfoResponse()
    {
      Id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
      Username = User.FindFirst(ClaimTypes.Name).Value,
      IsAdmin = User.FindAll(ClaimTypes.Role).Any(role => role.Value == UserRoles.Admin)
    };

    return Ok(new { user = info });
  }
}

public class GetAuthenticatedUserInfoResponse
{
  public int Id { get; set; }
  public string Username { get; set; }
  public bool IsAdmin { get; set; }
}