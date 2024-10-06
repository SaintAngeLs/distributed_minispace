using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class OrganizationImageUploaded : IEvent
    {
        public Guid OrganizationId { get; }
        public string ImageUrl { get; }
        public string ImageType { get; }
        public DateTime UploadDate { get; }

        public OrganizationImageUploaded(Guid organizationId, string imageUrl, string imageType, DateTime uploadDate)
        {
            OrganizationId = organizationId;
            ImageUrl = imageUrl;
            ImageType = imageType;
            UploadDate = uploadDate;
        }
    }
}
