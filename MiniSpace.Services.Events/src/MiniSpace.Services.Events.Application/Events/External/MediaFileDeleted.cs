using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Events.Application.Events.External
{
    [Message("mediafiles")]
    public class MediaFileDeleted: IEvent
    {
        public Guid MediaFileId { get; }
        public Guid SourceId { get; }
        public string Source { get; }

        public MediaFileDeleted(Guid mediaFileId, Guid sourceId, string source)
        {
            MediaFileId = mediaFileId;
            SourceId = sourceId;
            Source = source;
        }
    }
}