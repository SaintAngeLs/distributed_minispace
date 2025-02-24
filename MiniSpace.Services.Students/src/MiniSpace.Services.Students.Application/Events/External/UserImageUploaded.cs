using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("mediafiles")]
    public class UserImageUploaded : IEvent
    {
        public Guid UserId { get; }
        public string ImageUrl { get; }
        public string ImageType { get; }
        public DateTime UploadDate { get; }

        public UserImageUploaded(Guid userId, string imageUrl, string imageType, DateTime uploadDate)
        {
            UserId = userId;
            ImageUrl = imageUrl;
            ImageType = imageType;
            UploadDate = uploadDate;
        }
    }
}
