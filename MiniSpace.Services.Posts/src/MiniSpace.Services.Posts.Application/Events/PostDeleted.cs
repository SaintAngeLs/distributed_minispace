using Convey.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostDeleted : IEvent
    {
        public Guid PostId { get; }
        public IEnumerable<Guid> MediaFilesIds { get; }

        public PostDeleted(Guid postId, IEnumerable<Guid> mediaFilesIds)
        {
            PostId = postId;
            MediaFilesIds = mediaFilesIds;
        }
    }
}
