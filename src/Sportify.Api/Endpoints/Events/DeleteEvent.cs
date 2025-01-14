using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Sportify.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportify.Api.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Sportify.Api.Endpoints.Events;

public class DeleteEvent : EndpointBaseAsync
  .WithRequest<DeleteEventRequest>
  .WithActionResult
{
  private readonly SportifyDbContext _context;

  public DeleteEvent(SportifyDbContext context)
  {
    _context = context;
  }

  [Authorize]
  [HttpDelete("/api/events/delete/{id:int}")]
  [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
  public override async Task<ActionResult> HandleAsync([FromRoute] DeleteEventRequest request, CancellationToken cancellationToken = default)
  {
    int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    var @event = await _context.Events
        .Where(e => e.Id == request.Id && e.CreatorId == userId)
        .FirstOrDefaultAsync();

    if (@event != null)
    {
      try
      {
        _context.Remove(@event);
        await _context.SaveChangesAsync();
      }
      catch (Exception)
      {
        throw;
      }

      return Ok();
    }

    return NotFound();
  }
}

public class DeleteEventRequest
{
  [FromRoute]
  public int Id { get; set; }
}
