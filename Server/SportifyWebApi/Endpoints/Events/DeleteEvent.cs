using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Mvc;

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

        [HttpDelete("api/events/delete/{id}")]
        public override async Task<ActionResult> HandleAsync([FromRoute] DeleteEventRequest request, CancellationToken cancellationToken = default)
        {
            var @event = await _context.Events.FindAsync(request.Id);

            _context.Remove(@event);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }
    }

    public class DeleteEventRequest
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
