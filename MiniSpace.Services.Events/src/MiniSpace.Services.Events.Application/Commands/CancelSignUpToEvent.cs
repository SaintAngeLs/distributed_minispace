using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class CancelSignUpToEvent : ICommand
    {
        public Guid EventId { get; set; }
        public Guid StudentId { get; set; }
        
        public CancelSignUpToEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}