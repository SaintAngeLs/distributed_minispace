using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("comments")]
    public class CommentCreated : IEvent
    {
        public Guid CommentId { get; }

        public CommentCreated(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
