using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Events.Application.Events.External
{
    [Message("mediafiles")]
    public class MediaFileDeleted : IEvent
    {
        public string MediaFileUrl { get; }
        public Guid SourceId { get; }
        public string Source { get; }
        public Guid UploaderId { get; }
        public Guid? OrganizationId { get; }

        public MediaFileDeleted(string mediaFileUrl, Guid sourceId, string source, 
                                Guid uploaderId, Guid? organizationId)
        {
            MediaFileUrl = mediaFileUrl;
            SourceId = sourceId;
            Source = source;
            UploaderId = uploaderId;
            OrganizationId = organizationId;
        }
    }
}