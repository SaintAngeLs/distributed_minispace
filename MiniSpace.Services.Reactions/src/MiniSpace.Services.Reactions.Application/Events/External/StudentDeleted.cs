using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Reactions.Application.Events.External
{
    [Message("students")]
    public class StudentDeleted : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }

        public StudentDeleted(Guid studentId, string fullName)
        {
            StudentId = studentId;
            FullName = fullName;
        }
    }    
}
