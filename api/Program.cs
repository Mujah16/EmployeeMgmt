using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using api.Data;
using api.Repositories;
using api.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

try
{
var builder = WebApplication.CreateBuilder(args);

// Detect if running in Azure (production)
var isAzure = builder.Environment.IsProduction();

if (isAzure)
{
    var keyVaultName = "empmgmtkv29478";
    var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
    builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
}

var storageConn = builder.Configuration["StorageConnectionString"];
if (string.IsNullOrEmpty(storageConn))
{
    Console.WriteLine("StorageConnectionString is missing!");
}

// Register services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration["DbConnectionString"]));
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<EmployeeService>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
    c.OperationFilter<FileUploadOperation>(); // <- Add this
});
var app = builder.Build();

// Log actual config source at runtime
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation(isAzure
    ? "🔐 Using Azure Key Vault for configuration."
    : "⚙️ Using local appsettings.json for configuration.");

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("⚠️ Error loading from Azure Key Vault: " + ex.Message);
    throw;
}