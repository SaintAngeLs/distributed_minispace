using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class EventFileUploaded : IEvent
    {
        public Guid FileId { get; }
        public Guid EventId { get; }
        public string FileName { get; }
        public string FileUrl { get; }
        public string ContentType { get; }
        public DateTime UploadDate { get; }
        public Guid UploaderId { get; }

        public EventFileUploaded(Guid fileId, Guid eventId, string fileName, string fileUrl, 
                                 string contentType, DateTime uploadDate, Guid uploaderId)
        {
            FileId = fileId;
            EventId = eventId;
            FileName = fileName;
            FileUrl = fileUrl;
            ContentType = contentType;
            UploadDate = uploadDate;
            UploaderId = uploaderId;
        }
    }
}
