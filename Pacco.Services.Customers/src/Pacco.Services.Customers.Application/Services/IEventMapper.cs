using System.Collections.Generic;
using Convey.CQRS.Events;
using Pacco.Services.Customers.Core;

namespace Pacco.Services.Customers.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }
}