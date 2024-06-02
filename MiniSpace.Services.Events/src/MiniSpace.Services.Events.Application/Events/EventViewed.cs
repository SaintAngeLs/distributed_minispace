using System;
using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class EventViewed(Guid eventId) : IEvent
    {
        public Guid EventId { get; set; } = eventId;
    }
}