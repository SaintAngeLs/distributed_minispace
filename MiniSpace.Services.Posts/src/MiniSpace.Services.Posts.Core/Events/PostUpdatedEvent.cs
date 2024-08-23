using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Core.Events
{
    public class PostUpdatedEvent : IDomainEvent
    {
        public Guid PostId { get; }
        public DateTime OccurredOn { get; }

        public PostUpdatedEvent(Guid postId)
        {
            PostId = postId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}