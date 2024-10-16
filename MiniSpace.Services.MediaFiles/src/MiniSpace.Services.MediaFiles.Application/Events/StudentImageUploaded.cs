using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class StudentImageUploaded : IEvent
    {
        public Guid StudentId { get; }
        public string ImageUrl { get; }
        public string ImageType { get; }
        public DateTime UploadDate { get; }

        public StudentImageUploaded(Guid studentId, string imageUrl, string imageType, DateTime uploadDate)
        {
            StudentId = studentId;
            ImageUrl = imageUrl;
            ImageType = imageType;
            UploadDate = uploadDate;
        }
    }
}
