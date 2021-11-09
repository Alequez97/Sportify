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

namespace SportifyWebApi.Endpoints.City
{
    public class GetCities : BaseAsyncEndpoint
        .WithRequest<GetCitiesRequest>
        .WithResponse<GetCitiesResponse>
    {
        private readonly SportifyDbContext _context;

        public GetCities(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("/api/cities/{countryId}")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Cities })]
        public override async Task<ActionResult<GetCitiesResponse>> HandleAsync([FromRoute] GetCitiesRequest request, CancellationToken cancellationToken = default)
        {
            var res = await _context.Cities.Where(c => c.Country.Id == request.CountryId).Select(c => new GetCitiesResponse.CityDto()
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

            return res != null ? Ok(res) : NotFound();
        }
    }

    public class GetCitiesRequest
    {
        [FromRoute] public int CountryId { get; set; }
    }

    public class GetCitiesResponse
    {
        public List<CityDto> Cities { get; set; }

        public class CityDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
