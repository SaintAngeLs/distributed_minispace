using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    public class DeleteUserRejected : IRejectedEvent
    {
        [ExcludeFromCodeCoverage]
        public Guid UserId { get; }
        public string Reason { get; }
        public string Code { get; }
        
        public DeleteUserRejected(Guid userId, string reason, string code)
        {
            UserId = userId;
            Reason = reason;
            Code = code;
        }
    }    
}
