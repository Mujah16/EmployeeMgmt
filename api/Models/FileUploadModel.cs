using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class FileUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
