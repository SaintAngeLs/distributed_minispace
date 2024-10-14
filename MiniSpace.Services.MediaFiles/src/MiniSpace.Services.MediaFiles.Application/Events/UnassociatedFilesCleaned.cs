using Paralax.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class UnassociatedFilesCleaned: IEvent
    {
        public DateTime OccurredAt { get; }
        
        public UnassociatedFilesCleaned(DateTime occurredAt)
        {
            OccurredAt = occurredAt;
        }
    }
}