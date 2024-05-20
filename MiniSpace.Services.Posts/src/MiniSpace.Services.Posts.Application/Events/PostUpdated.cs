using Convey.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostUpdated : IEvent
    {
        public Guid PostId { get; }
        public IEnumerable<Guid> MediaFilesIds { get; }

        public PostUpdated(Guid postId, IEnumerable<Guid> mediaFilesIds)
        {
            PostId = postId;
            MediaFilesIds = mediaFilesIds;
        }
    }
}
