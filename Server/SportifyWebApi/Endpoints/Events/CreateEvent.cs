using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities;
using Microsoft.AspNetCore.Mvc;

namespace SportifyWebApi.Endpoints.Events
{
    public class CreateEvent : BaseAsyncEndpoint
        .WithRequest<CreateEventRequest>
        .WithoutResponse
    {
        private readonly SportifyDbContext _context;

        public CreateEvent(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpPost("api/events/create")]
        public override async Task<ActionResult> HandleAsync([FromQuery] CreateEventRequest request, CancellationToken cancellationToken = default)
        {
            _context.Events.Add(request.Event);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                return BadRequest();
            }

            return Ok();
        }
    }

    public class CreateEventRequest
    {
        [FromBody] public Event Event { get; set; }
    }
}
