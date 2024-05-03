using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("identity")]
    public class UserUnbanned(Guid userId) : IEvent
    {
        public Guid UserId { get; } = userId;
    }
}
