using Convey.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    public class UpdateStudentRejected : IRejectedEvent
    {
        [ExcludeFromCodeCoverage]
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
