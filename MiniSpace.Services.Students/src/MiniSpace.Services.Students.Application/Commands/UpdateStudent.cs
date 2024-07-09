using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateStudent : ICommand
    {
        public Guid StudentId { get; }
        public string ProfileImageUrl { get; }
        public string Description { get; }
        public bool EmailNotifications { get; }
        public string? BannerUrl { get; }
        public IEnumerable<string> GalleryOfImageUrls { get; }
        public string Education { get; }
        public string WorkPosition { get; }
        public string Company { get; }
        public IEnumerable<string> Languages { get; }
        public IEnumerable<string> Interests { get; }
        public bool EnableTwoFactor { get; }
        public bool DisableTwoFactor { get; }
        public string TwoFactorSecret { get; }
        public string? ContactEmail { get; }

        public UpdateStudent(Guid studentId, string profileImageUrl, string description, bool emailNotifications,
            string? bannerUrl, IEnumerable<string> galleryOfImageUrls, string education, string workPosition, 
            string company, IEnumerable<string> languages, IEnumerable<string> interests,
            bool enableTwoFactor, bool disableTwoFactor, string twoFactorSecret, string? contactEmail)
        {
            StudentId = studentId;
            ProfileImageUrl = profileImageUrl;
            Description = description;
            EmailNotifications = emailNotifications;
            BannerUrl = bannerUrl;
            GalleryOfImageUrls = galleryOfImageUrls ?? Enumerable.Empty<string>();
            Education = education;
            WorkPosition = workPosition;
            Company = company;
            Languages = languages ?? Enumerable.Empty<string>();
            Interests = interests ?? Enumerable.Empty<string>();
            EnableTwoFactor = enableTwoFactor;
            DisableTwoFactor = disableTwoFactor;
            TwoFactorSecret = twoFactorSecret;
            ContactEmail = contactEmail;
        }
    }    
}
