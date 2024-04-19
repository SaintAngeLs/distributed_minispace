using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventBackgroundWorkerStarted(string name) : IEvent
    {
        public string Name { get; set; } = name;
    }
}

