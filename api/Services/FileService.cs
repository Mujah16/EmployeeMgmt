using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace api.Services
{
    public class FileService : IFileService
    {
        private readonly BlobContainerClient _containerClient;

        public FileService(IConfiguration configuration)
        {
            var connectionString = configuration["StorageConnectionString"];
            var containerName = configuration["BlobContainerName"];
            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<(bool Success, string FileUrl, string ErrorMessage)> UploadFileAsync(IFormFile file)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(file.FileName);

                await using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

                return (true, blobClient.Uri.ToString(), null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<List<string>> ListAsync()
        {
            var blobs = new List<string>();
            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }
            return blobs;
        }
    }
}
