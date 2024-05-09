using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Comments.Application.Events
{
    public class CommentUpdated : IEvent
    {
        public Guid CommentId { get; }

        public CommentUpdated(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
