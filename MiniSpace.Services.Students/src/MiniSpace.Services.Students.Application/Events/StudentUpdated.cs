using Convey.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentUpdated : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }
        public string ProfileImageUrl { get; }
        public string BannerUrl { get; }
        public IEnumerable<string> GalleryOfImageUrls { get; }
        public string Education { get; }
        public string WorkPosition { get; }
        public string Company { get; }
        public IEnumerable<string> Languages { get; }
        public IEnumerable<string> Interests { get; }
        public string ContactEmail { get; } // New property

        public StudentUpdated(Guid studentId, string fullName, string profileImageUrl, string bannerUrl,
                              IEnumerable<string> galleryOfImageUrls, string education, string workPosition,
                              string company, IEnumerable<string> languages, IEnumerable<string> interests,
                              string contactEmail)
        {
            StudentId = studentId;
            FullName = fullName;
            ProfileImageUrl = profileImageUrl;
            BannerUrl = bannerUrl;
            GalleryOfImageUrls = galleryOfImageUrls;
            Education = education;
            WorkPosition = workPosition;
            Company = company;
            Languages = languages;
            Interests = interests;
            ContactEmail = contactEmail;
        }
    }
}
