using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities;
using Microsoft.AspNetCore.Mvc;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Map
{
    public class SaveNewLocation : BaseAsyncEndpoint
        .WithRequest<SportsGroundLocationtRequest>
        .WithoutResponse
    {
        private readonly SportifyDbContext _context;

        public SaveNewLocation(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpPost("api/map/save")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Map })]
        public override async Task<ActionResult> HandleAsync([FromBody] SportsGroundLocationtRequest request, CancellationToken cancellationToken = default)
       {
            var geolocation = new Geolocation()
            {
                Latitude = request.Lat.ToString(),
                Longitude = request.Lng.ToString()
            };
            _context.Geolocations.Add(geolocation);

            var location = new SportsGroundLocation()
            {
                Country = request.Country,
                City = request.City,
                District = request.District,
                Address = $"{request.Street}, {request.HouseNumber}",
                TypeId = request.TypeId,
                Geolocation = geolocation
            };

            _context.SportsGroundLocations.Add(location);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }

    public class SportsGroundLocationtRequest
    {
        public int TypeId { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}
