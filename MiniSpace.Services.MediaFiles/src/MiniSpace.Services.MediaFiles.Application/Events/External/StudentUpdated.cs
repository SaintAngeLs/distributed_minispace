using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("students")]
    public class StudentUpdated : IEvent
    {
        // public Guid StudentId { get; }
        // public string ProfileImageUrl { get; }
        // public string BannerUrl { get; }
        // public IEnumerable<string> GalleryOfImageUrls { get; }
       
        // public StudentUpdated(Guid studentId, string profileImageUrl, string bannerUrl,
        //                       IEnumerable<string> galleryOfImageUrls)
        // {
        //     StudentId = studentId;
        //     ProfileImageUrl = profileImageUrl;
        //     BannerUrl = bannerUrl;
        //     GalleryOfImageUrls = galleryOfImageUrls;
        // }
    }  
}
