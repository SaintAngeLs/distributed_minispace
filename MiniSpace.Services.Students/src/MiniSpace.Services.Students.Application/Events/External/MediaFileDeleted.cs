using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("mediafiles")]
    public class MediaFileDeleted : IEvent
    {
        public string MediaFileUrl { get; }
        public Guid SourceId { get; }
        public string Source { get; }
        public Guid UploaderId { get; }

        public MediaFileDeleted(string mediaFileUrl, Guid sourceId, string source, Guid uploaderId)
        {
            MediaFileUrl = mediaFileUrl;
            SourceId = sourceId;
            Source = source;
            UploaderId = uploaderId;
        }
    }
}
