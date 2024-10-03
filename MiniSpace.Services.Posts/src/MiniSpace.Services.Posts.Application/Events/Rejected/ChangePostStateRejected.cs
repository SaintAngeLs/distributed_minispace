using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class ChangePostStateRejected : IRejectedEvent
    {
        public Guid PostId { get; }
        public string State { get; }
        public string Reason { get; }
        public string Code { get; }

        public ChangePostStateRejected(Guid postId, string state, string reason, string code)
        {
            PostId = postId;
            State = state;
            Reason = reason;
            Code = code;
        }
    }    
}
