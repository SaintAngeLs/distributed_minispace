using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Reports.Application.Events.External
{
    [Message("students")]
    public class StudentCreated : IEvent
    {
        public Guid StudentId { get; }
        public string Name { get; }

        public StudentCreated(Guid studentId, string name)
        {
            StudentId = studentId;
            Name = name;
        }
    }
}