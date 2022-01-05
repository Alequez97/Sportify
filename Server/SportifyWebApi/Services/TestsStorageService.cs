using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SportifyWebApi.Interfaces;

namespace SportifyWebApi.Services
{
    /// <summary>
    /// This class imitates image upload for integration tests
    /// </summary>
    public class TestsStorageService : IStorageService
    {
        public Task<string> UploadAsync(IFormFile file)
        {
            return WriteFileAsync(file);
        }

        private Task<string> WriteFileAsync(IFormFile file)
        {
            return Task.Run(() => {
                try
                {
                    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    var fileName = DateTime.Now.Ticks + extension; // Create a new Name for the file due to security reasons.

                    var path = Path.Combine("", fileName);

                    return path;
                }
                catch (Exception)
                {
                    //TODO: Log error
                    return null;
                }
            });
        }
    }
}
