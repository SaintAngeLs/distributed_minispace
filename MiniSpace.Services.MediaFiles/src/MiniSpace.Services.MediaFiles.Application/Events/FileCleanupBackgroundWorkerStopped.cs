using Paralax.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class FileCleanupBackgroundWorkerStopped: IEvent
    {
        public DateTime StoppedAt { get; }

        public FileCleanupBackgroundWorkerStopped(DateTime stoppedAt)
        {
            StoppedAt = stoppedAt;
        }
    }
}