using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.Specification.EntityFrameworkCore;
using DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyWebApi.Constants;
using SportifyWebApi.Specifications;
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
            var result = await _context.Events
                .WithSpecification(new FilterEventsSpec(request.CategoryId, request.CountryId, request.CityId))
                .Select(x => new GetEventsResponse()
            {
                Id = x.Id,
                Title = x.Title,
                CategoryName = x.Category.Name,
                BriefDesc = x.BriefDesc,
                CreatorName = x.Creator.UserName,
                CreatorId = x.CreatorId,
                Date = DateTime.SpecifyKind(x.Date, DateTimeKind.Utc).ToString("o"),
                IsGoing = User.Identity.IsAuthenticated && x.EventUsers.Any(xx => (xx.UserId == Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value) && xx.IsGoing)),
                IsCreator = User.Identity.IsAuthenticated && x.CreatorId == Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Contributors = x.EventUsers.Where(i => i.IsGoing == true).Select(xx => new GetEventsResponse.GetEventsContributorDto()
                {
                    Id = xx.User.Id,
                    Username = xx.User.UserName
                }).ToList(),
                Venue = new GetEventsResponse.GetEventsVenueDto()
                {
                    Country = x.Venue.Country.Name,
                    City = x.Venue.City.Name,
                    Address = x.Venue.Address
                }
            }).ToListAsync();

            return Ok(result);
        }
    }

    public class GetEventsRequest
    {
        [FromQuery(Name = "categoryId")] public int CategoryId { get; set; }
        [FromQuery(Name = "countryId")] public int CountryId { get; set; }
        [FromQuery(Name = "cityId")] public int CityId { get; set; }
    }

    public class GetEventsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string CategoryName { get; set; }

        public string BriefDesc { get; set; }

        public GetEventsVenueDto Venue { get; set; }

        public string Date { get; set; }

        public string CreatorName { get; set; }
        public int CreatorId { get; set; }

        public bool IsGoing { get; set; }

        public bool IsCreator { get; set; }

        public List<GetEventsContributorDto> Contributors { get; set; }

        public class GetEventsContributorDto
        {
            public int Id { get; set; }
            public string Username { get; set; }
        }

        public class GetEventsVenueDto
        {
            public string Country { get; set; }

            public string City { get; set; }

            public string Address { get; set; }
        }
    }
}
