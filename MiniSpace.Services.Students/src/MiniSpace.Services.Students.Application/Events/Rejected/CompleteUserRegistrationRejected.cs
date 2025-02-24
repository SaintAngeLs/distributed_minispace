using Paralax.CQRS.Events;
using MiniSpace.Services.Students.Application.Commands;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class CompleteUserRegistrationRejected : IRejectedEvent
    {
        public Guid UserId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CompleteUserRegistrationRejected(Guid userId, string reason, string code)
        {
            UserId = userId;
            Reason = reason;
            Code = code;
        }
    }    
}
