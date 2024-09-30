using System.Collections.Generic;
using Paralax.CQRS.Events;
using MiniSpace.Services.Events.Core;

namespace MiniSpace.Services.Events.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }
}