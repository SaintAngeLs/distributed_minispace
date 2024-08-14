using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Posts.Application.Events.External
{
    [Message("mediafiles")]
    public class MediaFileDeleted : IEvent
    {
        public string MediaFileUrl { get; }
        public Guid SourceId { get; }
        public string Source { get; }
        public Guid UploaderId { get; }
        public Guid? OrganizationId { get; }
        public Guid? EventId { get; }
        public Guid? PostId { get; }

        public MediaFileDeleted(string mediaFileUrl, Guid sourceId, string source, 
                                Guid uploaderId, Guid? organizationId,
                                Guid? eventId = null, Guid? postId = null)
        {
            MediaFileUrl = mediaFileUrl;
            SourceId = sourceId;
            Source = source;
            UploaderId = uploaderId;
            OrganizationId = organizationId;
            EventId = eventId;
            PostId = postId;
        }
    }
}
