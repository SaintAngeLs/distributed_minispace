using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class PostFileUploaded : IEvent
    {
        public Guid FileId { get; }
        public Guid PostId { get; }
        public Guid? OrganizationId { get; }
        public Guid? EventId { get; } 
        public string FileName { get; }
        public string FileUrl { get; }
        public string ContentType { get; }
        public DateTime UploadDate { get; }
        public Guid UploaderId { get; }

        public PostFileUploaded(Guid fileId, Guid postId, Guid? organizationId, Guid? eventId, 
                                string fileName, string fileUrl, string contentType, 
                                DateTime uploadDate, Guid uploaderId)
        {
            FileId = fileId;
            PostId = postId;
            OrganizationId = organizationId; 
            EventId = eventId;
            FileName = fileName;
            FileUrl = fileUrl;
            ContentType = contentType;
            UploadDate = uploadDate;
            UploaderId = uploaderId;
        }
    }
}
