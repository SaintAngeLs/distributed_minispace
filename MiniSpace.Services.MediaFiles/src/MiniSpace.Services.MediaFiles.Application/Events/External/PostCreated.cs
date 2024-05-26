using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("posts")]
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