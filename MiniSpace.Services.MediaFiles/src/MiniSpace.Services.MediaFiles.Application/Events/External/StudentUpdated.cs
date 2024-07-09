using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("students")]
    public class StudentUpdated : IEvent
    {
        public Guid StudentId { get; }
        public Guid MediaFileId { get; }
        public Guid BannerMediaFileId { get; }
        public IEnumerable<Guid> GalleryOfImages { get; }
  

        public StudentUpdated(Guid studentId, Guid mediaFileId, Guid bannerMediaFileId,
                              IEnumerable<Guid> galleryOfImages)
        {
            StudentId = studentId;
            MediaFileId = mediaFileId;
            BannerMediaFileId = bannerMediaFileId;
            GalleryOfImages = galleryOfImages;
        }
    }  
}
