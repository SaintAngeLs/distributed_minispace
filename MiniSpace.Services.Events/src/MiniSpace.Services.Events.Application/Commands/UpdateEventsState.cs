using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class UpdateEventsState(DateTime now) : ICommand
    {
        public DateTime Now { get; set; } = now;
    }
}