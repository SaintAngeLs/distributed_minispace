using Convey.CQRS.Events;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core;
using MiniSpace.Services.Organizations.Core.Events;

namespace MiniSpace.Services.Organizations.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case StudentRegistrationCompleted e:
                    return new Application.Events.StudentCreated(e.Student.Id, e.Student.FullName);
                case StudentUpdated e:
                    return new Application.Events.StudentUpdated(e.Student.Id, e.Student.FullName);
                case StudentStateChanged e:
                    return new Application.Events.StudentStateChanged(e.Student.Id, e.Student.FullName,
                        e.Student.State.ToString().ToLowerInvariant(), e.PreviousState.ToString().ToLowerInvariant());
            }

            return null;
        }
    }
}
