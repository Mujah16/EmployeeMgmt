using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ProcessFileFunction.services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Retrieve Key Vault URI from environment variable
        var keyVaultUriStr = Environment.GetEnvironmentVariable("KeyVaultUri");
        if (string.IsNullOrEmpty(keyVaultUriStr))
        {
            throw new InvalidOperationException("Missing KeyVaultUri in environment variables.");
        }
        // Connect to Key Vault
        var secretClient = new SecretClient(new Uri(keyVaultUriStr), new DefaultAzureCredential());
        var secret = secretClient.GetSecret("StorageConnectionString");
        var storageConnectionString = secret.Value.Value;

        // Register BlobServiceClient and FileProcessor
        services.AddSingleton(new BlobServiceClient(storageConnectionString));
        services.AddSingleton<IFileProcessor, FileProcessor>();
    })
    .Build();

await host.RunAsync();
