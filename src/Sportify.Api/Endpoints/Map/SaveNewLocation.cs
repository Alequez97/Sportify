using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Sportify.DataServices;
using Sportify.DomainEntities.SportsGroundEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sportify.Api.Constants;
using Sportify.Api.Interfaces;
using Sportify.Api.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Sportify.Api.Endpoints.Map;

public class SaveNewLocation : EndpointBaseAsync
  .WithRequest<SportsGroundSaveNewLocationRequest>
  .WithActionResult<SportsGroundSaveNewLocationResponse>
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
    try
    {
      var location = new SportsGroundLocation()
      {
        Country = request.Country,
        City = request.City,
        District = request.District,
        Street = request.Street,
        HouseNumber = request.HouseNumber,
        TypeId = (int)request.TypeId,
        Latitude = (double)request.Lat,
        Longitude = (double)request.Lng,
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

      await _context.SaveChangesAsync();

      var response = new SportsGroundSaveNewLocationResponse()
      {
        Id = location.Id,
        Images = location.Images?.Select(image => image.Path).ToList(),
        Message = "New location successfully saved",
        Status = ResponseStatus.Success
      };

      return Ok(response);
    }
    catch (Exception ex)
    {
      // TODO: Log exception
      return StatusCode(500, new ResponseBase() { Message = ex.InnerException.Message, Status = "Error" });
    }
  }
}

public class SportsGroundSaveNewLocationRequest
{
  [Required]
  public int? TypeId { get; set; }

  public string Description { get; set; }

  public string Country { get; set; }

  public string City { get; set; }

  public string District { get; set; }

  public string Street { get; set; }

  public string HouseNumber { get; set; }

  [Required]
  public double? Lat { get; set; }

  [Required]
  public double? Lng { get; set; }

  public List<IFormFile> Images { get; set; }
}

public class SportsGroundSaveNewLocationResponse : ResponseBase
{
  public int Id { get; set; }

  public List<string> Images { get; set; }
}
