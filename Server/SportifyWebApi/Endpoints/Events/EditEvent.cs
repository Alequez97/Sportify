using System;
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
    public class EditEvent : BaseAsyncEndpoint
        .WithRequest<EditEventRequest>
        .WithoutResponse
    {
        private readonly SportifyDbContext _context;

        public EditEvent(SportifyDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPut("/api/event/edit/{id}")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult> HandleAsync([FromBody] EditEventRequest request, CancellationToken cancellationToken = default)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var @event = await _context.Events
                .Where(e => e.Id == request.Id)
                .Include(v => v.Venue)
                .Include(v => v.EventUsers.Where(u => u.UserId == userId))
                .FirstOrDefaultAsync();

            if(@event != null)
            {
                @event.Title = request.Title;
                @event.Description = request.Description;
                @event.BriefDesc = request.BriefDesc;
                @event.CategoryId = request.CategoryId;
                @event.Venue.CountryId = request.CountryId;
                @event.Venue.CityId = request.CityId;
                @event.Venue.Address = request.Address;
                @event.Date = request.Date;
                @event.Venue.Latitude = request.Lat;
                @event.Venue.Longitude = request.Lng;
                if(@event.EventUsers.Count == 0 && request.IsGoing == true)
                {
                    @event.EventUsers
                    .Add(new EventUser()
                    {
                        EventId = request.Id,
                        UserId = userId,
                        IsGoing = true
                    });
                }
                else
                {
                    var record = @event.EventUsers.FirstOrDefault();
                    if (record != null) record.IsGoing = request.IsGoing;
                }
            }

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
        public string Title { get; set; }

        [FromBody]
        public int CategoryId { get; set; }

        [FromBody]
        public string BriefDesc { get; set; }

        [FromBody]
        public string Description { get; set; }

        [FromBody]
        public int CountryId { get; set; }

        [FromBody]
        public int CityId { get; set; }

        [FromBody]
        public string Address { get; set; }

        [FromBody]
        public double Lat { get; set; }

        [FromBody]
        public double Lng { get; set; }

        [FromBody]
        public DateTime Date { get; set; }

        [FromBody]
        public bool IsGoing { get; set; }
    }
}
