using Paralax.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostBackgroundWorkerStopped(string name) : IEvent
    {
        public string Name { get; set; } = name;
    }
}
