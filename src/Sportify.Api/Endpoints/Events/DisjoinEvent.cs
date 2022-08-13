using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.Specification.EntityFrameworkCore;
using Sportify.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sportify.Api.Constants;
using Swashbuckle.AspNetCore.Annotations;
using Sportify.Api.Specifications;
using System.ComponentModel.DataAnnotations;

namespace Sportify.Api.Endpoints.Events;

public class DisjoinEvent : EndpointBaseAsync
  .WithRequest<DisjoinEventRequest>
  .WithActionResult
{
  private readonly SportifyDbContext _context;

  public DisjoinEvent(SportifyDbContext context)
  {
    _context = context;
  }

  [Authorize]
  [HttpPost("/api/events/disjoin")]
  [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
  public override async Task<ActionResult> HandleAsync(DisjoinEventRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
      var record = _context.EventUsers.WithSpecification(new EventUserByIdSpec((int)request.EventId, userId)).FirstOrDefault();

      if (record != null)
      {
        record.IsGoing = false;
      }
      else return NotFound();

      await _context.SaveChangesAsync();

      return Ok();
    }
    catch (Exception)
    {
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }
}

public class DisjoinEventRequest
{
  [Required]
  public int? EventId { get; set; }
}
