using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Sportify.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportify.Api.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Sportify.Api.Endpoints.Country
{
  public class GetCountries : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<GetCountriesResponse>
  {
    private readonly SportifyDbContext _context;

    public GetCountries(SportifyDbContext context)
    {
      _context = context;
    }

    [HttpGet("/api/countries")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Countries })]
    public override async Task<ActionResult<GetCountriesResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
      var res = await _context.Countries.Select(x => new GetCountriesResponse()
      {
        Id = x.Id,
        Name = x.Name
      })
      .ToListAsync();

      return Ok(res != null ? res : NotFound());
    }
  }

  public class GetCountriesResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}
