using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace ProcessFileFunction.services
{
    public class FileProcessor : IFileProcessor
    {
        private readonly BlobServiceClient _blobServiceClient;

        public FileProcessor(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> ProcessFileAsync(Stream inputStream, string fileName)
        {
            using var reader = new StreamReader(inputStream);
            var content = await reader.ReadToEndAsync();
            var processedContent = content.ToUpperInvariant();

            var bytes = Encoding.UTF8.GetBytes(processedContent);
            using var outputStream = new MemoryStream(bytes);

            var containerClient = _blobServiceClient.GetBlobContainerClient("processed");
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(outputStream, overwrite: true);

            return processedContent;
        }
    }
}