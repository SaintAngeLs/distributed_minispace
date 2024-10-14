using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class DeletePostRejected : IRejectedEvent
    {
        public Guid PostId { get; }
        public string Reason { get; }
        public string Code { get; }

        public DeletePostRejected(Guid postId, string reason, string code)
        {
            PostId = postId;
            Reason = reason;
            Code = code;
        }
    }    
}
