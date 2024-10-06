using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using MiniSpace.Services.Email.Core.Entities;

namespace MiniSpace.Services.Email.Application.Events.External
{
    [Contract]
    public class UserStatusChanged : IEvent
    {
        public Guid UserId { get; }
        public string NewStatus { get; }

        public UserStatusChanged(Guid userId, string newStatus)
        {
            UserId = userId;
            NewStatus = newStatus;
        }
    }
}
