using Convey.CQRS.Events;
using System;
using System.Diagnostics.CodeAnalysis;


namespace MiniSpace.Services.Comments.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class LikeUpdated : IEvent
    {
        public Guid CommentId { get; }

        public LikeUpdated(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
