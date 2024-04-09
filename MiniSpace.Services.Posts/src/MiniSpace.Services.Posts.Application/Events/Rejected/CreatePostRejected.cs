using Convey.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events.Rejected
{
    public class CreatePostRejected : IRejectedEvent
    {
        public Guid PostId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CreatePostRejected(Guid postId, string reason, string code)
        {
            PostId = postId;
            Reason = reason;
            Code = code;
        }
    }    
}
