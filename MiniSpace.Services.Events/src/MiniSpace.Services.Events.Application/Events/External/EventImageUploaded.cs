using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Events.Application.Events.External
{
    [Message("mediafiles")]
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
