using Convey.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events.Rejected
{
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
