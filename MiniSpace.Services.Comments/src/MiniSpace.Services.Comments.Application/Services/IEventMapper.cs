using Convey.CQRS.Events;
using MiniSpace.Services.Comments.Core.Events;
using System.Collections.Generic;

namespace MiniSpace.Services.Comments.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }
}
