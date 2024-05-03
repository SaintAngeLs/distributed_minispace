using Convey.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events.Rejected
{
    public class DeleteStudentRejected : IRejectedEvent
    {
        public Guid StudentId { get; }
        public string Reason { get; }
        public string Code { get; }
        
        public DeleteStudentRejected(Guid studentId, string reason, string code)
        {
            StudentId = studentId;
            Reason = reason;
            Code = code;
        }
    }    
}
