using Paralax.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class FileCleanupBackgroundWorkerStarted: IEvent
    {
        public DateTime StartedAt { get; }

        public FileCleanupBackgroundWorkerStarted(DateTime startedAt)
        {
            StartedAt = startedAt;
        }
    }
}