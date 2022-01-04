using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [HttpPost("/api/events/create")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult> HandleAsync([FromBody] CreateEventRequest request, CancellationToken cancellationToken = default)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var @event = new Event()
            {
                Title = request.Title,
                CategoryId = (int)request.CategoryId,
                BriefDesc = request.BriefDesc,
                Description = request.Description,
                Venue = new Venue
                {
                    CountryId = (int)request.CountryId,
                    CityId = (int)request.CityId,
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
            catch(Exception) //TODO: logging
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }

    public class CreateEventRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        [Required]
        public string BriefDesc { get; set; }

        public string Description { get; set; }

        [Required]
        public int? CountryId { get; set; }

        [Required]
        public int? CityId { get; set; }

        [Required]
        public string Address { get; set; }

        public double Lat { get; set; }
        
        public double Lng { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsGoing { get; set; }
    }
}
