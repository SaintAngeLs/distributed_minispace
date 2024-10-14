using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Commands
{
    public class UploadMediaFile : ICommand
    {
        public Guid MediaFileId { get; set; }
        public Guid SourceId { get; set; }
        public string SourceType { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid UploaderId { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public byte[] FileData { get; set; }

        public UploadMediaFile(Guid mediaFileId, Guid sourceId, string sourceType, Guid? organizationId,
            Guid uploaderId, string fileName, string fileContentType, byte[] fileData)
        {
            MediaFileId = mediaFileId == Guid.Empty ? Guid.NewGuid() : mediaFileId;
            SourceId = sourceId;
            SourceType = sourceType;
            OrganizationId = organizationId; 
            UploaderId = uploaderId;
            FileName = fileName;
            FileContentType = fileContentType;
            FileData = fileData;
        }
    }
}
