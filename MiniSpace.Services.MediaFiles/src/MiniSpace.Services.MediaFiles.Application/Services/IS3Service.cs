using System.IO;
using System.Threading.Tasks;

namespace MiniSpace.Services.MediaFiles.Application.Services
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(string fileName, Stream fileStream);
        Task DownloadFileAsync(string fileName, Stream destination);
        Task DeleteFileAsync(string fileName);
    }
}
