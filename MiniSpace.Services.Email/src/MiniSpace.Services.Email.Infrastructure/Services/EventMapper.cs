using System.Collections.Generic;
using System.Linq;
using Paralax.CQRS.Events;
using MiniSpace.Services.Email.Core.Events;
using MiniSpace.Services.Email.Application.Events.External;
using MiniSpace.Services.Email.Application.Events;
using MiniSpace.Services.Email.Application.Services;

using EmailSentCore = MiniSpace.Services.Email.Core.Events.EmailSent;

namespace MiniSpace.Services.Email.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case EmailCreated e:
                    return new EmailCreated(e.EmailNotificationId, e.UserId);
                case EmailDeleted e:
                    return new UserStatusChanged(e.UserId, "Deleted"); 
                case EmailSentCore e:
                    return new Application.Events.EmailSent(e.EmailNotificationId, e.UserId, e.SentAt);
                case Core.Events.NotificationCreated n:
                    return new MiniSpace.Services.Email.Application.Events.External.NotificationCreated(
                        n.NotificationId, 
                        n.UserId, 
                        n.Message, 
                        n.CreatedAt, 
                        n.EventType, 
                        n.RelatedEntityId, 
                        n.Details);
                default:
                    return null;
            }
        }
    }
}
