using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
    public class UserUnbanned(Guid userId) : IEvent
    {
        public Guid UserId { get; } = userId;
    }
}
