using System;
using System.Collections.Generic;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class RemoveEventParticipant: ICommand
    {
        public Guid EventId { get; set; }
        public Guid ParticipantId { get; set; }
    }
}