using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyWebApi.Constants;
using SportifyWebApi.Models;
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
                .Include(s => s.Type)
                .Include(s => s.Images)
                .Where(s => s.Latitude > request.Lat - request.Delta 
                         && s.Latitude < request.Lat + request.Delta
                         && s.Longitude > request.Lng - request.Delta
                         && s.Longitude < request.Lng + request.Delta);

            var locationsList = await locations.ToListAsync();

            var responseList = locationsList.Select(l => {
                var response = new SportsGroundGetLocationsResponse()
                {
                    Id = l.Id,
                    Lat = l.Latitude,
                    Lng = l.Longitude,
                    TypeId = l.Type.Id,
                    TypeName = l.Type.Name,
                    Description = l.Description,
                    Images = l.Images?.Select(image => image.Path).ToList()
                };

                return response;
            }).ToList();

            return responseList.Count > 0 ? Ok(responseList) : Ok(new ResponseBase() { Message = "No locations where found by given parameters", Status = ResponseStatus.NotFound });
        }
    }

    public class SportsGroundGetLocationsRequest
    {
        [Required]
        [FromQuery]
        public double? Lat { get; set; }

        [Required]
        [FromQuery]
        public double? Lng { get; set; }

        [Required]
        [FromQuery]
        public double? Delta { get; set; }
    }

    public class SportsGroundGetLocationsResponse
    {
        public int Id { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }

        public List<string> Images { get; set; }
    }
}
