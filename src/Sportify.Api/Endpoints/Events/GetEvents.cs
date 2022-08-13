using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.Specification.EntityFrameworkCore;
using Sportify.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportify.Api.Constants;
using Sportify.Api.Specifications;
using Swashbuckle.AspNetCore.Annotations;

namespace Sportify.Api.Endpoints.Events;

public class GetEvents : EndpointBaseAsync
  .WithRequest<GetEventsRequest>
  .WithActionResult<GetEventsResponse>
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
    try
    {
      var result = await _context.Events
      .WithSpecification(new FilterEventsSpec(request.CategoryId, request.CountryId, request.CityId))
      .Select(x => new GetEventsResponse()
      {
        Id = x.Id,
        Title = x.Title,
        CategoryId = x.CategoryId,
        CategoryName = x.Category.Name,
        BriefDesc = x.BriefDesc,
        Description = x.Description,
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
          CountryId = x.Venue.CountryId,
          Country = x.Venue.Country.Name,
          CityId = x.Venue.CityId,
          City = x.Venue.City.Name,
          Address = x.Venue.Address,
          Lat = x.Venue.Latitude,
          Lng = x.Venue.Longitude
        }
      }).ToListAsync();

      return Ok(result);
    }
    catch (Exception e)
    {
      return StatusCode(500, new { Message = e.Message });
    }

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

  public int CategoryId { get; set; }
  public string CategoryName { get; set; }

  public string BriefDesc { get; set; }

  public string Description { get; set; }

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
    public int CountryId { get; set; }
    public string Country { get; set; }

    public int CityId { get; set; }
    public string City { get; set; }

    public string Address { get; set; }

    public double? Lat { get; set; }

    public double? Lng { get; set; }
  }
}
