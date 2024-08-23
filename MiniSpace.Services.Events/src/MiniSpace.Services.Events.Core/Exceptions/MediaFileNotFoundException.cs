using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class MediaFileNotFoundException : DomainException
    {
        public override string Code { get; } = "media_file_not_found";
        public Guid MediaFileId { get; }
        public string MediaFileUrl { get; }
        public Guid EventId { get; }

        public MediaFileNotFoundException(Guid mediaFileId, Guid eventId) 
            : base($"Media file with ID: '{mediaFileId}' was not found for event with ID: {eventId}.")
        {
            MediaFileId = mediaFileId;
            EventId = eventId;
        }

          public MediaFileNotFoundException(string mediaFileUrl, Guid eventId) 
            : base($"Media file with ID: '{mediaFileUrl}' was not found for event with ID: {eventId}.")
        {
            MediaFileUrl = mediaFileUrl;
            EventId = eventId;
        }
    }
}