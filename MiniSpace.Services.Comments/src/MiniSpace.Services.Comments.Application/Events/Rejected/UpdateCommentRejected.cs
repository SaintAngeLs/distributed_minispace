using Convey.CQRS.Events;

namespace MiniSpace.Services.Comments.Application.Events.Rejected
{
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
