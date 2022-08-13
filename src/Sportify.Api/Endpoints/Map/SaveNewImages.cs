using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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

public class SaveNewImages : EndpointBaseAsync
  .WithRequest<SportsGroundSaveNewLocationImageRequest>
  .WithActionResult
{
  private readonly SportifyDbContext _context;
  private readonly IStorageService _storageService;

  public SaveNewImages(SportifyDbContext context, IStorageService storageService)
  {
    _context = context;
    _storageService = storageService;
  }

  [HttpPost("api/map/save-images")]
  [Authorize]
  [SwaggerOperation(Tags = new[] { SwaggerGroup.Map })]
  public override async Task<ActionResult> HandleAsync([FromForm] SportsGroundSaveNewLocationImageRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      foreach (var image in request.Images)
      {
        var storedPath = await _storageService.UploadAsync(image);
        var imageModel = new SportsGroundImage()
        {
          SportsGroundLocationId = (int)request.LocationId,
          Path = Path.GetFileName(storedPath)
        };
        _context.SportsGroundImages.Add(imageModel);
        await _context.SaveChangesAsync();
      }

      return Ok();
    }
    catch
    {
      //TODO: Log exception
      return StatusCode(500, new ResponseBase() { Message = "Server was not able to save images" });
    }

  }
}

public class SportsGroundSaveNewLocationImageRequest
{
  [Required]
  public int? LocationId { get; set; }

  public List<IFormFile> Images { get; set; }
}
