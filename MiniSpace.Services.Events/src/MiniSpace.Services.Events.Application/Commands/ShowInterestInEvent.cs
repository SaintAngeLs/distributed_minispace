using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class ShowInterestInEvent: ICommand
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }
        
        public ShowInterestInEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}