using Convey.CQRS.Events;
using System;


namespace MiniSpace.Services.Comments.Application.Events
{
    public class LikeDeleted : IEvent
    {
        public Guid CommentId { get; }

        public LikeDeleted(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
