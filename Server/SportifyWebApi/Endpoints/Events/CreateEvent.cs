using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public override async Task<ActionResult> HandleAsync([FromBody] CreateEventRequest request, CancellationToken cancellationToken = default)
        {
            var @event = new Event()
            {
                BriefDesc = request.BriefDesc,
                CategoryId = request.CategoryId,
            };

            _context.Events.Add(@event);

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
        [Required]
        public string Title { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BriefDesc { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public int VenueId { get; set; }

        [Required]
        public DateTime TimeOfTheEvent { get; set; }

        [Required]
        public int CreatorId { get; set; }
    }
}
