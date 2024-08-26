using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Events;

namespace MiniSpace.Services.Notifications.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case NotificationCreated e:
                    return new MiniSpace.Services.Notifications.Application.Events.External.NotificationCreated(
                         e.NotificationId, e.UserId, e.Message, e.CreatedAt, e.EventType, e.RelatedEntityId, e.Details);
                case NotificationUpdated e:
                    return new MiniSpace.Services.Notifications.Application.Events.External.NotificationUpdated(
                        e.NotificationId, e.UserId, e.NewStatus);
                case NotificationDeleted e:
                    return new MiniSpace.Services.Notifications.Application.Events.External.NotificationDeleted(
                         e.UserId, e.NotificationId);
                //  case StudentNotifications sn:
                //     return new MiniSpace.Services.Notifications.Application.Events.External.NotificationCreated(
                //         Guid.NewGuid(), 
                //         sn.StudentId, 
                //         "Student notification details here", 
                //         DateTime.UtcNow, 
                //         "StudentNotificationCreated", 
                //         null, 
                //         "Additional details can be included here");
            }

            return null;
        }
    }
}
