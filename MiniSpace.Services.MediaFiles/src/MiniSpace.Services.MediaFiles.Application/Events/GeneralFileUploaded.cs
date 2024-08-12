using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class GeneralFileUploaded : IEvent
    {
        public Guid FileId { get; }
        public string FileName { get; }
        public string FileUrl { get; }
        public string FileType { get; }
        public string ContentType { get; }
        public DateTime UploadDate { get; }
        public Guid? OrganizationId { get; }
        public Guid UploaderId { get; }

        public GeneralFileUploaded(Guid fileId, string fileName, string fileUrl, string fileType, 
                                   string contentType, DateTime uploadDate, Guid? organizationId, 
                                   Guid uploaderId)
        {
            FileId = fileId;
            FileName = fileName;
            FileUrl = fileUrl;
            FileType = fileType;
            ContentType = contentType;
            UploadDate = uploadDate;
            OrganizationId = organizationId;
            UploaderId = uploaderId;
        }
    }
}
