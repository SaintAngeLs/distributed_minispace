using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class DeleteEvent: ICommand
    {
        public Guid EventId { get; set; }
    }
}