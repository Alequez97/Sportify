using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SportifyWebApi.Interfaces;

namespace SportifyWebApi.Services
{
    public class FileSystemStorageService : IStorageService
    {
        private readonly string _storagePath;
        private readonly bool _createDirecotryIfNotExists;

        public FileSystemStorageService(string storagePath, bool createDirecotryIfNotExists = false)
        {
            _storagePath = storagePath;
            _createDirecotryIfNotExists = createDirecotryIfNotExists;
        }

        public Task<string> UploadAsync(IFormFile file)
        {
            return WriteFile(file);
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var fileName = DateTime.Now.Ticks + extension; // Create a new Name for the file due to security reasons.

                if (!Directory.Exists(_storagePath))
                {
                    if (_createDirecotryIfNotExists)
                    {
                        Directory.CreateDirectory(_storagePath);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException($"Storage directory {_storagePath} not found");
                    }
                }

                var path = Path.Combine(_storagePath, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return path;
            }
            catch (Exception)
            {
                //TODO: Log error
                return null;
            }
        }
    }
}
