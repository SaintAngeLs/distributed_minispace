using Convey.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class ChangeStudentStateRejected : IRejectedEvent
    {
        public Guid StudentId { get; }
        public string State { get; }
        public string Reason { get; }
        public string Code { get; }

        public ChangeStudentStateRejected(Guid studentId, string state, string reason, string code)
        {
            StudentId = studentId;
            State = state;
            Reason = reason;
            Code = code;
        }
    }    
}
