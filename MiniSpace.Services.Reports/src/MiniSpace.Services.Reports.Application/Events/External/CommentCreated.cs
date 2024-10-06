using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Reports.Application.Events.External
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