using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Sportify.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportify.Api.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Sportify.Api.Endpoints.Map;

  public class GetSportsGroundTypes : EndpointBaseAsync
      .WithoutRequest
      .WithActionResult<SportsGroundTypeResponse>
  {
      private readonly SportifyDbContext _context;

      public GetSportsGroundTypes(SportifyDbContext context)
      {
          _context = context;
      }

      [HttpGet("api/map/types")]
      [SwaggerOperation(Tags = new[] { SwaggerGroup.Map })]
      public override async Task<ActionResult<SportsGroundTypeResponse>> HandleAsync(CancellationToken cancellationToken = default)
      {
          var sportsGroundTypes = await _context.SportsGroundTypes.ToListAsync();
          var response = sportsGroundTypes.Select(s => new SportsGroundTypeResponse() { Id = s.Id, Name = s.Name }).ToList();

          return response.Count > 0 ? Ok(response) : NotFound();
      }
  }

  public class SportsGroundTypeResponse
  {
      public int Id { get; set; }

      public string Name { get; set; }
  }
