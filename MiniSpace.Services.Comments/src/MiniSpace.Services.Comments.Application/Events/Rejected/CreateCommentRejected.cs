using Convey.CQRS.Events;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class CreateCommentRejected : IRejectedEvent
    {
        public Guid CommentId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CreateCommentRejected(Guid commentId, string reason, string code)
        {
            CommentId = commentId;
            Reason = reason;
            Code = code;
        }
    }    
}
