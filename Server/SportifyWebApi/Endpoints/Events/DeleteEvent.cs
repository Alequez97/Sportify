using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Mvc;
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

        [HttpDelete("/api/event/delete/{id}")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult> HandleAsync([FromRoute] DeleteEventRequest request, CancellationToken cancellationToken = default)
        {
            var @event = _context.Events.FindAsync(request.Id);

            _context.Remove(@event);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
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
