using System.IO;
using System.Threading.Tasks;

namespace MiniSpace.Services.MediaFiles.Application.Services
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(string folderName, string fileName, Stream fileStream);
        Task DownloadFileAsync(string fileUrl, Stream destination);
        Task DeleteFileAsync(string fileUrl);
    }
}
