using Convey.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class EventBackgroundWorkerStopped(string name) : IEvent
    {
        public string Name { get; set; } = name;
    }
}