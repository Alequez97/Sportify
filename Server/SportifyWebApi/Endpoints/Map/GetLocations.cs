using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Map
{
    public class GetLocations : BaseAsyncEndpoint
        .WithRequest<SportsGroundGetLocationsRequest>
        .WithResponse<SportsGroundGetLocationsResponse>
    {
        private readonly SportifyDbContext _context;

        public GetLocations(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/map")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Map })]
        public override async Task<ActionResult<SportsGroundGetLocationsResponse>> HandleAsync([FromRoute] SportsGroundGetLocationsRequest request, CancellationToken cancellationToken = default)
        {
            var locations = _context.SportsGroundLocations
                .Include(s => s.Geolocation)
                .Include(s => s.Type)
                .Where(s => s.Geolocation.Latitude > request.Lat - request.Delta 
                         && s.Geolocation.Latitude < request.Lat + request.Delta
                         && s.Geolocation.Longitude > request.Lng - request.Delta
                         && s.Geolocation.Longitude < request.Lng + request.Delta);

            var locationsList = await locations.ToListAsync();

            var responseList = locationsList.Select(l => {
                var response = new SportsGroundGetLocationsResponse()
                {
                    Id = l.Id,
                    Lat = l.Geolocation.Latitude,
                    Lng = l.Geolocation.Longitude,
                    TypeId = l.Type.Id,
                    TypeName = l.Type.Name,
                    Description = l.Description
                };

                return response;
            }).ToList();

            return responseList.Count > 0 ? Ok(responseList) : Ok(new Response() { Message = "No locations where found by given parameters", Status = "Not found" });
        }
    }

    public class SportsGroundGetLocationsRequest
    {
        [FromQuery(Name = "lat")] 
        public double Lat { get; set; }

        [FromQuery(Name = "lng")] 
        public double Lng { get; set; }

        [FromQuery(Name = "delta")] 
        public double Delta { get; set; }
    }

    public class SportsGroundGetLocationsResponse
    {
        public int Id { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }
    }
}
