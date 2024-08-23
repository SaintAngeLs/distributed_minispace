using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Application.Dto;

namespace MiniSpace.Services.MediaFiles.Application.Services
{
    public interface IMediaFilesService
    {
        public Task<FileUploadResponseDto> UploadAsync(UploadMediaFile command);
        public Task<GeneralFileUploadResponseDto> UploadFileAsync(UploadFile command);
    }
}