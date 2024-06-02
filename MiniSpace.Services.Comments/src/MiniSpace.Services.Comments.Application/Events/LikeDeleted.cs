using Convey.CQRS.Events;
using System;
using System.Diagnostics.CodeAnalysis;


namespace MiniSpace.Services.Comments.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class LikeDeleted : IEvent
    {
        public Guid CommentId { get; }

        public LikeDeleted(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
