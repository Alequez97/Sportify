using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using DomainEntities.SportsGroundEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportifyWebApi.Constants;
using SportifyWebApi.Models;
using SportifyWebApi.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Map
{
    public class SaveNewImage : BaseAsyncEndpoint
        .WithRequest<SportsGroundSaveNewLocationImageRequest>
        .WithoutResponse
    {
        private readonly SportifyDbContext _context;
        private readonly IStorageService _storageService;

        public SaveNewImage(SportifyDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        [HttpPost("api/map/save-images")]
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
                        SportsGroundLocationId = request.LocationId,
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
        public int LocationId { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}
