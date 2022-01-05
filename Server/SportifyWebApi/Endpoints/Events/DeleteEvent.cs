using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Events
{
    public class DeleteEvent : BaseAsyncEndpoint
        .WithRequest<DeleteEventRequest>
        .WithoutResponse
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

            if(@event != null)
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
        //[Required]
        public int Id { get; set; }
    }
}
