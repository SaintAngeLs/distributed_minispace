using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
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
                default:
                    return null;
            }
        }
    }
}
