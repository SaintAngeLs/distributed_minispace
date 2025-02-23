using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("mediafiles")]
    public class UserImageUploaded : IEvent
    {
        public Guid StudentId { get; }
        public string ImageUrl { get; }
        public string ImageType { get; }
        public DateTime UploadDate { get; }

        public UserImageUploaded(Guid studentId, string imageUrl, string imageType, DateTime uploadDate)
        {
            StudentId = studentId;
            ImageUrl = imageUrl;
            ImageType = imageType;
            UploadDate = uploadDate;
        }
    }
}
