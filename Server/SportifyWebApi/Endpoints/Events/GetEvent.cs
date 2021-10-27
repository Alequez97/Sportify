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
    public class GetEvent : BaseAsyncEndpoint
        .WithRequest<GetEventRequest>
        .WithResponse<Event>
    {
        private readonly SportifyDbContext _context;

        public GetEvent(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/events/{id}")]
        public override async Task<ActionResult<Event>> HandleAsync([FromRoute] GetEventRequest request, CancellationToken cancellationToken = default)
        {
            return Ok(await _context.Events.FindAsync(request.Id));
        }
    }
    
    public class GetEventRequest
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
