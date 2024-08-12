using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class EventImageUploaded : IEvent
    {
        public Guid EventId { get; }
        public string ImageUrl { get; }
        public string ImageType { get; }
        public DateTime UploadDate { get; }

        public EventImageUploaded(Guid eventId, string imageUrl, string imageType, DateTime uploadDate)
        {
            EventId = eventId;
            ImageUrl = imageUrl;
            ImageType = imageType;
            UploadDate = uploadDate;
        }
    }
}
