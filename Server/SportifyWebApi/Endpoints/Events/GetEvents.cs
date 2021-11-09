using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Events
{
    public class GetEvents : BaseAsyncEndpoint
        .WithRequest<GetEventsRequest>
        .WithResponse<GetEventsResponse>
    {
        private readonly SportifyDbContext _context;

        public GetEvents(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("/api/events")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult<GetEventsResponse>> HandleAsync([FromRoute] GetEventsRequest request, CancellationToken cancellationToken = default)
        {
            //var q = _context.Events.Include(v => v.Venue);
            //if (!string.IsNullOrEmpty(request.SortBy))
            //{
            //    //q = q.OrderBy(x=)
            //}

            var result = await _context.Events
                .Select(x => new GetEventsResponse()
                {
                    Title = x.Title,
                    CategoryName = x.Category.Name,
                    BriefDesc = x.BriefDesc,
                    CreatorName = x.Creator.UserName,
                    CreatorId = x.CreatorId,
                    TimeOfTheEvent = x.TimeOfTheEvent.ToShortDateString(),
                    Contributors = x.EventUsers.Where(u => u.Event.Id == x.Id).Select(xx => new GetEventsResponse.GetEventsContributorDto()
                    {
                        Id = xx.User.Id,
                        UserName = xx.User.UserName
                    }).ToList(),
                    Venue = new GetEventsResponse.GetEventsVenueDto()
                    {
                        Country = x.Venue.Country.Name,
                        City = x.Venue.City.Name,
                        Address = x.Venue.Address
                    }
                })
                .ToListAsync();

            return Ok(result);
        }
    }

    public class GetEventsRequest
    {
        [FromQuery(Name ="sortBy")] public string SortBy { get; set; }
        [FromQuery(Name = "sortDesc")] public bool SortDesc { get; set; }
    }

    public class GetEventsResponse
    {
        public string Title { get; set; }

        public string CategoryName { get; set; }

        public string BriefDesc { get; set; }

        public GetEventsVenueDto Venue { get; set; }

        public string TimeOfTheEvent { get; set; }

        public string CreatorName { get; set; }
        public int CreatorId { get; set; }

        public List<GetEventsContributorDto> Contributors { get; set; }

        public class GetEventsContributorDto
        {
            public int Id { get; set; }
            public string UserName { get; set; }
        }

        public class GetEventsVenueDto
        {
            public string Country { get; set; }

            public string City { get; set; }

            public string Address { get; set; }
        }
    }
}
