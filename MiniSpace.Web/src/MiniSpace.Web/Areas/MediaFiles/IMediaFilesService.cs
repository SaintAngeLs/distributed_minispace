using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.MediaFiles
{
    public interface IMediaFilesService
    {
        public Task<FileDto> GetFileAsync(Guid fileId);
        public Task<FileDto> GetOriginalFileAsync(Guid fileId);
        public Task<HttpResponse<FileUploadResponseDto>> UploadMediaFileAsync(
            Guid sourceId, 
            string sourceType, 
            Guid uploaderId, 
            string fileName,
            string fileContentType, 
            byte[] fileData);
        
        Task<HttpResponse<GeneralFileUploadResponseDto>> UploadFileAsync(
            Guid sourceId, 
            string sourceType, 
            Guid uploaderId, 
            string fileName,
            string fileContentType, 
            byte[] fileData);
            
        public Task<HttpResponse<FileUploadResponseDto>> UploadOrganizationImageAsync(
            Guid organizationId, 
            string sourceType, 
            Guid uploaderId, 
            string fileName, 
            string fileContentType, 
            byte[] fileData);
        public Task DeleteMediaFileAsync(string fileUrl);

        
    }
}