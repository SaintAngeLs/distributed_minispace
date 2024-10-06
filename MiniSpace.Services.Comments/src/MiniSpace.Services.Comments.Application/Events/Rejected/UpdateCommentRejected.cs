using Paralax.CQRS.Events;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class UpdateCommentRejected : IRejectedEvent
    {
        public Guid CommentId { get; }
        public string Reason { get; }
        public string Code { get; }

        public UpdateCommentRejected(Guid commentId, string reason, string code)
        {
            CommentId = commentId;
            Reason = reason;
            Code = code;
        }
    }    
}
