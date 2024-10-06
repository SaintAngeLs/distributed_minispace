using System;
using System.Collections.Generic;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class AddEventParticipant: ICommand
    {
        public Guid EventId { get; set; }
        public Guid StudentId { get; set; }
    }
}