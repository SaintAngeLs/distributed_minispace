using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External
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
