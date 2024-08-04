using MiniSpace.Services.MediaFiles.Application.Dto;
using MiniSpace.Services.MediaFiles.Core.Entities;
using System;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static FileSourceInfo AsEntity(this FileSourceInfoDocument document)
            => new FileSourceInfo(
                document.Id,
                document.SourceId,
                document.SourceType,
                document.UploaderId,
                document.State,
                document.CreatedAt,
                document.OriginalFileUrl,
                document.OriginalFileContentType,
                document.FileUrl,
                document.FileName,
                document.OrganizationId 
            );

        public static FileSourceInfoDocument AsDocument(this FileSourceInfo entity)
            => new FileSourceInfoDocument
            {
                Id = entity.Id,
                SourceId = entity.SourceId,
                SourceType = entity.SourceType,
                UploaderId = entity.UploaderId,
                State = entity.State,
                CreatedAt = entity.CreatedAt,
                OriginalFileUrl = entity.OriginalFileUrl,
                OriginalFileContentType = entity.OriginalFileContentType,
                FileUrl = entity.FileUrl,
                FileName = entity.FileName,
                OrganizationId = entity.OrganizationId
            };

    }
}
