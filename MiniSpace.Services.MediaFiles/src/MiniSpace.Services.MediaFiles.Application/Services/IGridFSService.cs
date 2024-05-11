using MongoDB.Bson;

namespace MiniSpace.Services.MediaFiles.Application.Services
{
    public interface IGridFSService
    {
        Task<ObjectId> UploadFileAsync(string fileName, Stream fileStream);
        Task DownloadFileAsync(ObjectId fileId, Stream destination);
        Task DeleteFileAsync(ObjectId fileId);
    }
}