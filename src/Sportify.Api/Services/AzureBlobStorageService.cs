using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Sportify.Api.Interfaces;

namespace Sportify.Api.Services;

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

    return Task.Run(() => "This is best code ever");
  }
}
