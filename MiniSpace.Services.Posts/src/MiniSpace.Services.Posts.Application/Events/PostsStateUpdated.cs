using Convey.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostsStateUpdated(DateTime updateDateTime) : IEvent
    {
        public DateTime UpdateDateTime { get; set; } = updateDateTime;
    }
}
