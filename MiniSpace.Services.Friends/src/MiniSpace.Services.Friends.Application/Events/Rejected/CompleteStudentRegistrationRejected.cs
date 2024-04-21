using Convey.CQRS.Events;
using MiniSpace.Services.Students.Application.Commands;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    public class CompleteStudentRegistrationRejected : IRejectedEvent
    {
        public Guid StudentId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CompleteStudentRegistrationRejected(Guid studentId, string reason, string code)
        {
            StudentId = studentId;
            Reason = reason;
            Code = code;
        }
    }    
}
