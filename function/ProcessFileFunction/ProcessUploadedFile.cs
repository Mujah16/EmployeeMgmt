using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ProcessFileFunction
{
    public class ProcessUploadedFile
    {
        private readonly BlobServiceClient _blobServiceClient;

        public ProcessUploadedFile(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        [Function("ProcessUploadedFile")]
        public async Task Run(
            [BlobTrigger("documents/{name}", Connection = "StorageConnectionString")] Stream myBlob,
            string name,
            FunctionContext context)
        {
            var logger = context.GetLogger("ProcessUploadedFile");

            // Copy stream to memory to access its length safely
            using var memoryStream = new MemoryStream();
            await myBlob.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            logger.LogInformation($"✅ Processed blob\n➡️ Name: {name}\n📦 Size: {memoryStream.Length} Bytes");

            // Copy blob to 'processed' container
            var processedContainer = _blobServiceClient.GetBlobContainerClient("processed");
            await processedContainer.CreateIfNotExistsAsync();

            var blobClient = processedContainer.GetBlobClient(name);
            await blobClient.UploadAsync(memoryStream, overwrite: true);

            logger.LogInformation("📁 File has been copied to the 'processed' container.");
        }
    }
}
