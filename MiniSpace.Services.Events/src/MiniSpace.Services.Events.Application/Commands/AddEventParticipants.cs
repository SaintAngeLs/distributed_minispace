using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class AddEventParticipants: ICommand
    {
        public Guid EventId { get; set; }
        public string EventEngagementType { get; set; }
        public IEnumerable<Guid> Participants { get; set; }
    }
}