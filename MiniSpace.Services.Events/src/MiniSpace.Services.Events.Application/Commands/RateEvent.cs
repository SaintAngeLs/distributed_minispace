using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class RateEvent : ICommand
    {
        public Guid EventId { get; set; }
        public int Rating { get; set; }
        public Guid StudentId { get; set; }
    }
}