using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class CancelInterestInEvent: ICommand
    {
        public Guid EventId { get; set; }
        public Guid StudentId { get; set; }
        
        public CancelInterestInEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}