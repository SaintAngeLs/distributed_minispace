using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("comments")]
    public class CommentUpdated : IEvent
    {
        public Guid CommentId { get; }

        public CommentUpdated(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
