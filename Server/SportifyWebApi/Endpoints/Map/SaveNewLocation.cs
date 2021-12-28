using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities.SportsGroundEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportifyWebApi.Constants;
using SportifyWebApi.Models;
using SportifyWebApi.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Map
{
    public class SaveNewLocation : BaseAsyncEndpoint
        .WithRequest<SportsGroundSaveNewLocationRequest>
        .WithResponse<SportsGroundSaveNewLocationResponse>
    {
        private readonly SportifyDbContext _context;
        private readonly IStorageService _storageservice;

        public SaveNewLocation(SportifyDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageservice = storageService;
        }

        [HttpPost("api/map/save")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Map })]
        public override async Task<ActionResult<SportsGroundSaveNewLocationResponse>> HandleAsync([FromForm] SportsGroundSaveNewLocationRequest request, CancellationToken cancellationToken = default)
        {
            var location = new SportsGroundLocation()
            {
                Country = request.Country,
                City = request.City,
                District = request.District,
                Street = request.Street,
                HouseNumber = request.HouseNumber,
                TypeId = request.TypeId,
                Latitude = request.Lat,
                Longitude = request.Lng,
                Description = request.Description,
                CreatorId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };

            if (request.Images != null)
            {
                location.Images = new List<SportsGroundImage>();
                foreach (var requestImage in request.Images)
                {
                    var savedFilePath = await _storageservice.UploadAsync(requestImage);
                    if (savedFilePath != null)
                    {
                        location.Images.Add(new SportsGroundImage() { Path = Path.GetFileName(savedFilePath) });
                    }
                }
            }

            _context.SportsGroundLocations.Add(location);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                // TODO: Log exception
                return StatusCode(500);
            }

            var response = new SportsGroundSaveNewLocationResponse()
            {
                Id = location.Id,
                Images = location.Images?.Select(image => image.Path).ToList(),
                Message = "New location successfully saved",
                Status = ResponseStatus.Success
            };
            return Ok(response);
        }
    }

    public class SportsGroundSaveNewLocationRequest
    {
        [Required]
        public int TypeId { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        [Required]
        public double Lat { get; set; }
        
        [Required]
        public double Lng { get; set; }

        public List<IFormFile> Images { get; set; }
    }

    public class SportsGroundSaveNewLocationResponse : ResponseBase
    {
        public int Id { get; set; }

        public List<string> Images { get; set; }
    }
}
