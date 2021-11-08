using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Mvc;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Events
{
    public class GetEvent : BaseAsyncEndpoint
        .WithRequest<GetEventRequest>
        .WithResponse<GetEventResponse>
    {
        private readonly SportifyDbContext _context;

        public GetEvent(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/event/{id}")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult<GetEventResponse>> HandleAsync([FromRoute] GetEventRequest request, CancellationToken cancellationToken = default)
        {
            var @event = await _context.Events.FindAsync(request.Id);

            return @event != null ? Ok(@event) : NotFound();
        }
    }

    public class GetEventRequest
    {
        [FromRoute]
        public int Id { get; set; }
    }

    public class GetEventResponse
    {
        public string Title { get; set; }

        public string CategoryName { get; set; }

        public string BriefDesc { get; set; }

        public string Description { get; set; }

        public GetEventVenueDto Venue { get; set; }

        public DateTime TimeOfTheEvent { get; set; }

        public int CreatorId { get; set; }

        public List<GetEventContributorDto> Contributors { get; set; }

        public class GetEventContributorDto
        {
            public string Email { get; set; }
            //and so on
        }

        public class GetEventVenueDto
        {
            public string Country { get; set; }

            public string City { get; set; }

            public string Address { get; set; }
        }
    }
}
