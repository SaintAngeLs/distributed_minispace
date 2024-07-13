using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.MediaFiles
{
    public class MediaFilesService : IMediaFilesService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;
        
        public MediaFilesService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }
        
        public Task<FileDto> GetFileAsync(Guid fileId)
        {
            return _httpClient.GetAsync<FileDto>($"media-files/{fileId}");
        }
        
        public Task<FileDto> GetOriginalFileAsync(Guid fileId)
        {
            return _httpClient.GetAsync<FileDto>($"media-files/{fileId}/original");
        }

        public Task<HttpResponse<FileUploadResponseDto>> UploadMediaFileAsync(
            Guid sourceId, 
            string sourceType, 
            Guid uploaderId, 
            string fileName,
            string fileContentType, 
            string base64Content)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, FileUploadResponseDto>("media-files", new { 
                MediaFileId = Guid.NewGuid(), 
                SourceId = sourceId,
                SourceType = sourceType,
                UploaderId = uploaderId,
                FileName = fileName,
                FileContentType = fileContentType,
                Base64Content = base64Content 
            });
        }
        
        public Task DeleteMediaFileAsync(Guid fileId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"media-files/{fileId}");
        }
        
    }
}