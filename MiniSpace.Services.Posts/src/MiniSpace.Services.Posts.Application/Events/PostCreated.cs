using Convey.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostCreated : IEvent
    {
        public Guid PostId { get; }
        public IEnumerable<string> MediaFilesUrls { get; }

        public PostCreated(Guid postId, IEnumerable<string> mediaFilesUrls)
        {
            PostId = postId;
            MediaFilesUrls = mediaFilesUrls;
        }
    }
}
