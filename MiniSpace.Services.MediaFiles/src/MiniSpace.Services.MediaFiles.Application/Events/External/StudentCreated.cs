using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("students")]
    public class StudentCreated : IEvent
    {
        public Guid StudentId { get; }
        public Guid MediaFileId { get; }

        public StudentCreated(Guid studentId, Guid mediaFileId)
        {
            StudentId = studentId;
            MediaFileId = mediaFileId;
        }
    }  
}