using Convey.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class MediaFileDeleted: IEvent
    {
        public Guid MediaFileId { get; }

        public MediaFileDeleted(Guid mediaFileId)
        {
            MediaFileId = mediaFileId;
        }
    }
}