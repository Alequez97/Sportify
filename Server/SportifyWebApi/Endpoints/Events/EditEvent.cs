using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using DataServices;
using DomainEntities;
using Microsoft.AspNetCore.Mvc;

namespace SportifyWebApi.Endpoints.Events
{
    public class EditEvent : BaseAsyncEndpoint
        .WithRequest<EditEventRequest>
        .WithoutResponse
    {
        private readonly SportifyDbContext _context;
        private IMapper _mapper;

        public EditEvent(SportifyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPut("api/events/edit/{id}")]
        public override async Task<ActionResult> HandleAsync([FromBody] EditEventRequest request, CancellationToken cancellationToken = default)
        {
            request.Event.Id = request.Id;
            var @event = await _context.Events.FindAsync(request.Id);

            _mapper.Map(request.Event, @event);

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

    public class EditEventRequest
    {
        [FromRoute] 
        public int Id { get; set; }
        [FromBody]
        public Event Event { get; set; }
    }
}
