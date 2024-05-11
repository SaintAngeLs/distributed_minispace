using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Comments.Application.Events.External
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
