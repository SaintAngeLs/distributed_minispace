using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Comments.Application.Events.External
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
