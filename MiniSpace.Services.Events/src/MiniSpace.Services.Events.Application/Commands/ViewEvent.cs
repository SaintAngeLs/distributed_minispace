using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class ViewEvent : ICommand
    {
        public Guid UserId { get; }
        public Guid EventId { get; }

        public ViewEvent(Guid userId, Guid eventId)
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}
