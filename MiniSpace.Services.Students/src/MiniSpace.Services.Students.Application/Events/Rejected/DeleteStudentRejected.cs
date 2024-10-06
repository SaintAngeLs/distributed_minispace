using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    public class DeleteStudentRejected : IRejectedEvent
    {
        [ExcludeFromCodeCoverage]
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
