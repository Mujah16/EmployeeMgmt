using System.IO;
using System.Threading.Tasks;

namespace ProcessFileFunction.services
{
    public interface IFileProcessor
    {
        Task<string> ProcessFileAsync(Stream inputStream, string fileName);
    }
}
