using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Comments.Application.Events
{
    public class CommentDeleted : IEvent
    {
        public Guid CommentId { get; }

        public CommentDeleted(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
