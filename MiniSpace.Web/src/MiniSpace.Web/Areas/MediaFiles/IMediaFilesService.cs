using System;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.MediaFiles
{
    public interface IMediaFilesService
    {
        public Task<FileDto> GetFileAsync(Guid fileId);
        public Task<FileDto> GetOriginalFileAsync(Guid fileId);
        public Task<HttpResponse<object>> UploadMediaFileAsync(Guid sourceId, string sourceType, Guid uploaderId,
            string fileName, string fileContentType, string base64Content);
        public Task DeleteMediaFileAsync(Guid fileId);
    }
}