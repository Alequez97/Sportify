using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpPost("/api/event/create")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult> HandleAsync([FromBody] CreateEventRequest request, CancellationToken cancellationToken = default)
        {
            var @event = new Event()
            {
                Title = request.Title,
                CategoryId = request.CategoryId,
                BriefDesc = request.BriefDesc,
                Description = request.Description,
                Venue = new Venue
                {
                    CountryId = request.CountryId,
                    CityId = request.CityId,
                    Address = request.Address
                },
                TimeOfTheEvent = request.TimeOfTheEvent,
                CreatorId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };

            _context.Events.Add(@event);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }

    public class CreateEventRequest
    {
        public string Title { get; set; }

        public int CategoryId { get; set; }

        public string BriefDesc { get; set; }

        public string Description { get; set; }

        public int CountryId { get; set; }

        public int CityId { get; set; }

        public string Address { get; set; }

        public DateTime TimeOfTheEvent { get; set; }
    }
}
