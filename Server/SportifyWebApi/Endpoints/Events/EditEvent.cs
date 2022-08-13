using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities.EventEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Events
{
    public class EditEvent : EndpointBaseAsync
        .WithRequest<EditEventRequest>
        .WithActionResult
    {
        private readonly SportifyDbContext _context;

        public EditEvent(SportifyDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPut("/api/events/edit")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult> HandleAsync([FromBody] EditEventRequest request, CancellationToken cancellationToken = default)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var @event = await _context.Events
                .Where(e => e.Id == request.Id && e.CreatorId == userId)
                .Include(v => v.Venue)
                .Include(v => v.EventUsers.Where(u => u.UserId == userId))
                .FirstOrDefaultAsync();

            if (@event != null)
            {
                @event.Title = request.Title;
                @event.Description = request.Description;
                @event.BriefDesc = request.BriefDesc;
                @event.CategoryId = (int)request.CategoryId;
                @event.Venue.CountryId = (int)request.CountryId;
                @event.Venue.CityId = (int)request.CityId;
                @event.Venue.Address = request.Address;
                @event.Date = request.Date;
                @event.Venue.Latitude = request.Lat;
                @event.Venue.Longitude = request.Lng;
                if (@event.EventUsers.Count == 0 && request.IsGoing == true)
                {
                    @event.EventUsers
                    .Add(new EventUser()
                    {
                        EventId = (int)request.Id,
                        UserId = userId,
                        IsGoing = true
                    });
                }
                else
                {
                    var record = @event.EventUsers.FirstOrDefault();
                    if (record != null) record.IsGoing = request.IsGoing;
                }

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
            
            return NotFound();   
        }
    }

    public class EditEventRequest
    {
        [FromBody]
        [Required]
        public int? Id { get; set; }

        [FromBody]
        [Required]
        public string Title { get; set; }

        [FromBody]
        [Required]
        public int? CategoryId { get; set; }

        [FromBody]
        [Required]
        public string BriefDesc { get; set; }

        [FromBody]
        public string Description { get; set; }

        [FromBody]
        [Required]
        public int? CountryId { get; set; }

        [FromBody]
        [Required]
        public int? CityId { get; set; }

        [FromBody]
        [Required]
        public string Address { get; set; }

        [FromBody]
        public double Lat { get; set; }

        [FromBody]
        public double Lng { get; set; }

        [FromBody]
        [Required]
        public DateTime Date { get; set; }

        [FromBody]
        [Required]
        public bool IsGoing { get; set; }
    }
}
