using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class SignUpToEvent : ICommand
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public SignUpToEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}