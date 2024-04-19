using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Posts.Application.Events.External
{
    [Message("students")]
    public class StudentCreated : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }

        public StudentCreated(Guid studentId, string fullName)
        {
            StudentId = studentId;
            FullName = fullName;
        }
    }    
}
