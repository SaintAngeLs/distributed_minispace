using System.Collections.Generic;
using Convey.CQRS.Events;
using Pacco.Services.Deliveries.Core.Events;

namespace Pacco.Services.Deliveries.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }
}