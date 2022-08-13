using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sportify.Api.Interfaces;

  /// <summary>
  /// Service that uploads images
  /// </summary>
  public interface IStorageService
  {
      /// <summary>
      /// Method that uploads images
      /// </summary>
      /// <param name="file"></param>
      /// <returns>Path if file was successfully uploaded, otherwise returns null</returns>
      Task<string> UploadAsync(IFormFile file);
  }
