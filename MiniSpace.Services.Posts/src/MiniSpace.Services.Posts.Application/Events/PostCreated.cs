using Convey.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostCreated : IEvent
    {
        public Guid PostId { get; }
        public IEnumerable<Guid> MediaFilesIds { get; }

        public PostCreated(Guid postId, IEnumerable<Guid> mediaFilesIds)
        {
            PostId = postId;
            MediaFilesIds = mediaFilesIds;
        }
    }
}
