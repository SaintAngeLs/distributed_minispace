using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Comments.Application.Events
{
    public class CommentCreated : IEvent
    {
        public Guid CommentId { get; }

        public CommentCreated(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
