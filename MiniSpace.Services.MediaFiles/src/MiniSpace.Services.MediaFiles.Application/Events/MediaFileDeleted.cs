using Convey.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
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