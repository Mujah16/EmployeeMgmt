using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace api.Services
{
    public interface IFileService
    {
        Task<(bool Success, string FileUrl, string ErrorMessage)> UploadFileAsync(IFormFile file);
        Task<List<string>> ListAsync();
    }
}
