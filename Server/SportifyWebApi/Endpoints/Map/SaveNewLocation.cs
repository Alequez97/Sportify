using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities;
using Microsoft.AspNetCore.Mvc;
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
        [SwaggerOperation(Tags = new[] { "Map" })]
        public override async Task<ActionResult> HandleAsync([FromBody] SportsGroundLocationtRequest request, CancellationToken cancellationToken = default)
        {
            var geolocation = new Geolocation()
            {
                Latitude = request.Lat,
                Longitude = request.Lng
            };

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

        public int Description { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }
    }
}
