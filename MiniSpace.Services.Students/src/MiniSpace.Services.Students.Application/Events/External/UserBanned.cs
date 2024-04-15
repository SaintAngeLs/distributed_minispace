using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
    public class UserBanned(Guid userId) : IEvent
    {
        public Guid UserId { get; } = userId;
    }
}
