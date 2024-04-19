using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventBackgroundWorkerStopped(string name) : IEvent
    {
        public string Name { get; set; } = name;
    }
}