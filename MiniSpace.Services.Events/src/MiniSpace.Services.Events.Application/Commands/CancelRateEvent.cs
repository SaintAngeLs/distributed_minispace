using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class CancelRateEvent: ICommand
    {
        public Guid EventId { get; set; }
        public Guid StudentId { get; set; }
    }
}