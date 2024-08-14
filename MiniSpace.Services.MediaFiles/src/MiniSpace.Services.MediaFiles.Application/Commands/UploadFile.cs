using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Commands
{
    public class UploadFile : ICommand
    {
        public Guid FileId { get; set; }
        public Guid SourceId { get; set; }
        public string SourceType { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? EventId { get; set; }  
        public Guid UploaderId { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public byte[] FileData { get; set; }

        public UploadFile(Guid fileId, Guid sourceId, string sourceType, Guid? organizationId, Guid? eventId,
            Guid uploaderId, string fileName, string fileContentType, byte[] fileData)
        {
            FileId = fileId == Guid.Empty ? Guid.NewGuid() : fileId;
            SourceId = sourceId;
            SourceType = sourceType;
            OrganizationId = organizationId;
            EventId = eventId;
            UploaderId = uploaderId;
            FileName = fileName;
            FileContentType = fileContentType;
            FileData = fileData;
        }
    }
}
