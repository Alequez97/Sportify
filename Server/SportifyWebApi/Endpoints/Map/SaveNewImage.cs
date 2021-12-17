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
            foreach (var image in request.Images)
            {
                var storedPath = await _storageService.UploadAsync(image);
                var imageModel = new SportsGroundImage()
                {
                    SportsGroundLocationId = 1,
                    Path = Path.GetFileName(storedPath)
                };
                _context.SportsGroundImages.Add(imageModel);
                await _context.SaveChangesAsync();
            }
            
            return Ok();
        }
    }

    public class SportsGroundSaveNewLocationImageRequest
    {
        public List<IFormFile> Images { get; set; }
    }
}
