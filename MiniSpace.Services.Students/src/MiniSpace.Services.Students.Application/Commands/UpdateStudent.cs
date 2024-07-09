using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateStudent : ICommand
    {
        public Guid StudentId { get; }
        public Guid ProfileImage { get; }
        public string Description { get; }
        public bool EmailNotifications { get; }
        public Guid? BannerId { get; }
        public IEnumerable<Guid> GalleryOfImages { get; }
        public string Education { get; }
        public string WorkPosition { get; }
        public string Company { get; }
        public IEnumerable<string> Languages { get; }
        public IEnumerable<string> Interests { get; }
        public bool EnableTwoFactor { get; }
        public bool DisableTwoFactor { get; }
        public string TwoFactorSecret { get; }

        public UpdateStudent(Guid studentId, Guid profileImage, string description, bool emailNotifications,
            Guid? bannerId, IEnumerable<Guid> galleryOfImages, string education, string workPosition, 
            string company, IEnumerable<string> languages, IEnumerable<string> interests,
            bool enableTwoFactor, bool disableTwoFactor, string twoFactorSecret)
        {
            StudentId = studentId;
            ProfileImage = profileImage;
            Description = description;
            EmailNotifications = emailNotifications;
            BannerId = bannerId;
            GalleryOfImages = galleryOfImages ?? Enumerable.Empty<Guid>();
            Education = education;
            WorkPosition = workPosition;
            Company = company;
            Languages = languages ?? Enumerable.Empty<string>();
            Interests = interests ?? Enumerable.Empty<string>();
            EnableTwoFactor = enableTwoFactor;
            DisableTwoFactor = disableTwoFactor;
            TwoFactorSecret = twoFactorSecret;
        }
    }    
}
