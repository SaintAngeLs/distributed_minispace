using Paralax.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class MediaFileUploaded : IEvent
    {
        public Guid Id { get; }
        public string FileName { get; }

        public MediaFileUploaded(Guid id, string fileName)
        {
            Id = id;
            FileName = fileName;
        }
    }
}