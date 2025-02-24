using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    public class UpdateUserRejected : IRejectedEvent
    {
        [ExcludeFromCodeCoverage]
        public Guid UserId { get; }
        public string Reason { get; }
        public string Code { get; }
        
        public UpdateUserRejected(Guid userId, string reason, string code)
        {
            UserId = userId;
            Reason = reason;
            Code = code;
        }
    }
}
