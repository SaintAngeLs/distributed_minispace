using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External
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
