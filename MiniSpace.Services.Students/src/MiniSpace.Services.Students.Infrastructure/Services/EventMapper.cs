using Convey.CQRS.Events;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core;
using MiniSpace.Services.Students.Core.Events;

namespace MiniSpace.Services.Students.Infrastructure.Services
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
                    return new Application.Events.StudentCreated(e.Student.Id, e.Student.FullName, e.Student.ProfileImage);
                case StudentUpdated e:
                    return new Application.Events.StudentUpdated(e.Student.Id, e.Student.FullName, e.Student.ProfileImage);
                case StudentStateChanged e:
                    return new Application.Events.StudentStateChanged(e.Student.Id, e.Student.FullName,
                        e.Student.State.ToString().ToLowerInvariant(), e.PreviousState.ToString().ToLowerInvariant());
            }

            return null;
        }
    }
}
