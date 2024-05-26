using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("students")]
    public class StudentUpdated : IEvent
    {
        public Guid StudentId { get; }
        public Guid MediaFileId { get; }

        public StudentUpdated(Guid studentId, Guid mediaFileId)
        {
            StudentId = studentId;
            MediaFileId = mediaFileId;
        }
    }  
}