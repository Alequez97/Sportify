using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Sportify.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportify.Api.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Sportify.Api.Endpoints.Events;

public class GetEvent : EndpointBaseAsync
  .WithRequest<GetEventRequest>
  .WithActionResult<GetEventResponse>
{
  private readonly SportifyDbContext _context;

  public GetEvent(SportifyDbContext context)
  {
    _context = context;
  }

  [HttpGet("/api/events/{id}")]
  [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
  public override async Task<ActionResult<GetEventResponse>> HandleAsync([FromRoute] GetEventRequest request, CancellationToken cancellationToken = default)
  {
    var @event = await _context.Events.Where(e => e.Id == request.Id).Select(x => new GetEventResponse
    {
      Title = x.Title,
      CategoryName = x.Category.Name,
      BriefDesc = x.BriefDesc,
      Description = x.Description,
      Venue = new GetEventResponse.GetEventVenueDto()
      {
        Country = x.Venue.Country.Name,
        City = x.Venue.City.Name,
        Address = x.Venue.Address,
        Lat = x.Venue.Latitude,
        Lng = x.Venue.Longitude
      },
      Date = DateTime.SpecifyKind(x.Date, DateTimeKind.Utc).ToString("o"),
      CreatorUsername = x.Creator.UserName,
      CreatorId = x.CreatorId,
      Contributors = x.EventUsers.Where(i => i.IsGoing == true).Select(xx => new GetEventResponse.GetEventContributorDto()
      {
        Id = xx.User.Id,
        Username = xx.User.UserName
      }).ToList()
    }).FirstOrDefaultAsync();

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

  public string Date { get; set; }

  public string CreatorUsername { get; set; }

  public int CreatorId { get; set; }

  public List<GetEventContributorDto> Contributors { get; set; }

  public class GetEventContributorDto
  {
    public int Id { get; set; }
    public string Username { get; set; }
  }

  public class GetEventVenueDto
  {
    public string Country { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public double? Lat { get; set; }

    public double? Lng { get; set; }
  }
}
