using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    public class StudentImageUploaded : IEvent
    {
        public Guid StudentId { get; }
        public string ImageUrl { get; }
        public string ImageType { get; }

        public StudentImageUploaded(Guid studentId, string imageUrl, string imageType)
        {
            StudentId = studentId;
            ImageUrl = imageUrl;
            ImageType = imageType;
        }
    }
}
