using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("posts")]
    public class PostDeleted : IEvent
    {
        public Guid PostId { get; }

        public PostDeleted(Guid postId)
        {
            PostId = postId;
        }
    }  
}