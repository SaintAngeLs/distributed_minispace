using Convey.CQRS.Events;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    public class UpdateStudentRejected : IRejectedEvent
    {
        public Guid StudentId { get; }
        public string Reason { get; }
        public string Code { get; }
        
        public UpdateStudentRejected(Guid studentId, string reason, string code)
        {
            StudentId = studentId;
            Reason = reason;
            Code = code;
        }
    }
}
