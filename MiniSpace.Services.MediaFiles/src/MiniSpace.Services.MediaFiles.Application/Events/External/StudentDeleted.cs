using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("students")]
    public class StudentDeleted : IEvent
    {
        public Guid StudentId { get; }

        public StudentDeleted(Guid studentId)
        {
            StudentId = studentId;
        }
    }  
}