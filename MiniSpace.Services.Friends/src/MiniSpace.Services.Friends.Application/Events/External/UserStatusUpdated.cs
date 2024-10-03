using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Friends.Application.Events.External
{
    [Message("users")]
    public class UserStatusUpdated : IEvent
    {
        public Guid UserId { get; }
        public string Status { get; }

        public UserStatusUpdated(Guid userId, string status)
        {
            UserId = userId;
            Status = status;
        }
    }
}
