using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class CreateStudentRejected : IRejectedEvent
    {
        public Guid StudentId { get; }
        public string Reason { get; }
        public string Code { get; }
        
        public CreateStudentRejected(Guid studentId, string reason, string code)
        {
            StudentId = studentId;
            Reason = reason;
            Code = code;
        }
    }    
}
