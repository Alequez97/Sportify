using System;
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
                .Where(s => s.Country == request.Country);

            if (request.City != null)
            {
                locations = locations.Where(l => l.City == request.City);
            }
            if (request.District != null)
            {
                locations = locations.Where(l => l.District == request.District);
            }
            var locationsList = await locations.ToListAsync();

            var responseList = locationsList.Select(l => {
                var response = new SportsGroundGetLocationsResponse()
                {
                    Lat = Convert.ToDouble(l.Geolocation.Latitude),
                    Lng = Convert.ToDouble(l.Geolocation.Longitude),
                    Type = l.Type.Name,
                    Description = l.Description
                };

                return response;
            }).ToList();

            return responseList.Count > 0 ? Ok(responseList) : NotFound();
        }
    }

    public class SportsGroundGetLocationsRequest
    {
        [FromQuery(Name = "country")] 
        public string Country { get; set; }

        [FromQuery(Name = "city")] 
        public string City { get; set; }

        [FromQuery(Name = "district")] 
        public string District { get; set; }
    }

    public class SportsGroundGetLocationsResponse
    {
        public double Lat { get; set; }

        public double Lng { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
    }
}
