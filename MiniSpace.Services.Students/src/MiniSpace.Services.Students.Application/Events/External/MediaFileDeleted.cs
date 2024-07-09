using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("mediafiles")]
    public class MediaFileDeleted : IEvent
    {
        public string MediaFileUrl { get; }
        public Guid SourceId { get; }
        public string Source { get; }

        public MediaFileDeleted(string mediaFileUrl, Guid sourceId, string source)
        {
            MediaFileUrl = mediaFileUrl;
            SourceId = sourceId;
            Source = source;
        }
    }
}