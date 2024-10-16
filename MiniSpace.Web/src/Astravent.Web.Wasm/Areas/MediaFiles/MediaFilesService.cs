﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.MediaFiles
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
            byte[] fileData)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);

            var requestBody = new
            {
                MediaFileId = Guid.NewGuid(),
                SourceId = sourceId,
                SourceType = sourceType,
                UploaderId = uploaderId,
                FileName = fileName,
                FileContentType = fileContentType,
                FileData = fileData
            };

            return _httpClient.PostAsync<object, FileUploadResponseDto>("media-files", requestBody);
        }

        public Task<HttpResponse<GeneralFileUploadResponseDto>> UploadFileAsync(
            Guid sourceId, 
            string sourceType, 
            Guid uploaderId, 
            string fileName,
            string fileContentType, 
            byte[] fileData)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);

            var requestBody = new
            {
                FileId = Guid.NewGuid(),
                SourceId = sourceId,
                SourceType = sourceType,
                UploaderId = uploaderId,
                FileName = fileName,
                FileContentType = fileContentType,
                FileData = fileData
            };

            return _httpClient.PostAsync<object, GeneralFileUploadResponseDto>("media-files/files", requestBody);
        }
        
        public Task DeleteMediaFileAsync(string fileUrl)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"media-files/delete/{Uri.EscapeDataString(fileUrl)}", 
            new { MediaFileUrl = fileUrl });
        }

        public Task<HttpResponse<FileUploadResponseDto>> UploadOrganizationImageAsync(
            Guid organizationId,
            string sourceType,
            Guid uploaderId,
            string fileName,
            string fileContentType,
            byte[] fileData)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);

            var requestBody = new
            {
                OrganizationId = organizationId,
                MediaFileId = Guid.NewGuid(),
                SourceType = sourceType,
                UploaderId = uploaderId,
                FileName = fileName,
                FileContentType = fileContentType,
                FileData = fileData
            };

            return _httpClient.PostAsync<object, FileUploadResponseDto>("media-files", requestBody);
        }

      
    }
}