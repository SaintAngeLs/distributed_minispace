using System;
using System.Threading.Tasks;
using MiniSpacePwa.DTO;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.MediaFiles
{
    public interface IMediaFilesService
    {
        public Task<FileDto> GetFileAsync(Guid fileId);
        public Task<FileDto> GetOriginalFileAsync(Guid fileId);
        public Task<HttpResponse<FileUploadResponseDto>> UploadMediaFileAsync(Guid sourceId, string sourceType, 
            Guid uploaderId, string fileName, string fileContentType, string base64Content);
        public Task DeleteMediaFileAsync(Guid fileId);
    }
}
