using Convey.CQRS.Events;
using System;


namespace MiniSpace.Services.Comments.Application.Events
{
    public class LikeUpdated : IEvent
    {
        public Guid CommentId { get; }

        public LikeUpdated(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
