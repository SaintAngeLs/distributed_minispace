using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Comments.Application.Events.External
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
