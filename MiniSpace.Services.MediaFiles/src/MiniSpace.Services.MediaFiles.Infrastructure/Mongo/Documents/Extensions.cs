using MiniSpace.Services.MediaFiles.Core.Entities;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static FileSourceInfoDocument AsDocument(this FileSourceInfo fileSourceInfo)
            => new FileSourceInfoDocument
            {
                Id = fileSourceInfo.Id,
                SourceId = fileSourceInfo.SourceId,
                SourceType = fileSourceInfo.SourceType,
                UploaderId = fileSourceInfo.UploaderId,
                State = fileSourceInfo.State,
                CreatedAt = fileSourceInfo.CreatedAt,
                OriginalFileId = fileSourceInfo.OriginalFileId,
                FileId = fileSourceInfo.FileId,
                FileName = fileSourceInfo.FileName
            };

        public static FileSourceInfo AsEntity(this FileSourceInfoDocument document)
            => new FileSourceInfo(document.Id, document.SourceId, document.SourceType, document.UploaderId, 
                document.State, document.CreatedAt, document.OriginalFileId, document.FileId, document.FileName);
    }
}