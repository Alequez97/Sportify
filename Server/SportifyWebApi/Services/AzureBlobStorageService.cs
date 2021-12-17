using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace SportifyWebApi.Services
{
    public class AzureBlobStorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public Task<string> UploadAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(""); // TODO: Implement real Azure upload
            var blobClient = containerClient.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            blobClient.Upload(stream);

            return Task.Run<string>(() => "This is piece of shittable code. Never code like this. This sucks so much");
        }
    }
}