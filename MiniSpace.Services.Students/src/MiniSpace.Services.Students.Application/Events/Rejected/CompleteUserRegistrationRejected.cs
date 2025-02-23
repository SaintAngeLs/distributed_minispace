using Paralax.CQRS.Events;
using MiniSpace.Services.Students.Application.Commands;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class CompleteUserRegistrationRejected : IRejectedEvent
    {
        public Guid StudentId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CompleteUserRegistrationRejected(Guid studentId, string reason, string code)
        {
            StudentId = studentId;
            Reason = reason;
            Code = code;
        }
    }    
}
