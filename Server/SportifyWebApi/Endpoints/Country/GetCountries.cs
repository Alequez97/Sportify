using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SportifyWebApi.Endpoints.Country
{
    public class GetCountries : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<GetCountriesResponse>
    {
        private readonly SportifyDbContext _context;

        public GetCountries(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/countries")]
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
