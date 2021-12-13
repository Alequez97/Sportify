using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities.EventEntities;
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

        [Authorize]
        [HttpPost("/api/event/create")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult> HandleAsync([FromBody] CreateEventRequest request, CancellationToken cancellationToken = default)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

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
                    Address = request.Address,
                    Latitude = request.Lat,
                    Longitude = request.Lng
                },
                Date = request.Date,
                CreatorId = userId,
                EventUsers = request.IsGoing ? new List<EventUser> { new EventUser() { IsGoing = true, UserId = userId } } : null
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

        public double Lat { get; set; }
        
        public double Lng { get; set; }

        public DateTime Date { get; set; }

        public bool IsGoing { get; set; }
    }
}
