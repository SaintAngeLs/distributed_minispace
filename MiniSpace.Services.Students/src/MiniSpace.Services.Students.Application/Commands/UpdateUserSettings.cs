using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateUserSettings : ICommand
    {
        public Guid StudentId { get; set; }
        public string CreatedAtVisibility { get; set; }
        public string DateOfBirthVisibility { get; set; }
        public string InterestedInEventsVisibility { get; set; }
        public string SignedUpEventsVisibility { get; set; }
        public string EducationVisibility { get; set; }
        public string WorkPositionVisibility { get; set; }
        public string LanguagesVisibility { get; set; }
        public string InterestsVisibility { get; set; }
        public string ContactEmailVisibility { get; set; }
        public string PhoneNumberVisibility { get; set; }
        public string ProfileImageVisibility { get; set; }
        public string BannerImageVisibility { get; set; }
        public string GalleryVisibility { get; set; }

        public string ConnectionVisibility { get; set; }
        public string FollowersVisibility { get; set; }
        public string FollowingVisibility { get; set; }
        public string MyPostsVisibility { get; set; }
        public string ConnectionsPostsVisibility { get; set; }
        public string MyRepostsVisibility { get; set; }
        public string RepostsOfMyConnectionsVisibility { get; set; }
        public string OrganizationIAmCreatorVisibility { get; set; }
        public string OrganizationIFollowVisibility { get; set; }

        public string IsOnlineVisibility { get; set; }
        public string DeviceTypeVisibility { get; set; }
        public string LastActiveVisibility { get; set; }

        public string CountryVisibility { get; set; }
        public string CityVisibility { get; set; }

        public string PreferredLanguage { get; set; }
        public string FrontendVersion { get; set; }

        public UpdateUserSettings(Guid studentId, string createdAtVisibility, string dateOfBirthVisibility,
                                  string interestedInEventsVisibility, string signedUpEventsVisibility,
                                  string educationVisibility, string workPositionVisibility, 
                                  string languagesVisibility, string interestsVisibility, 
                                  string contactEmailVisibility, string phoneNumberVisibility, 
                                  string profileImageVisibility, string bannerImageVisibility, 
                                  string galleryVisibility, string preferredLanguage, string frontendVersion,
                                  string connectionVisibility, string followersVisibility, 
                                  string followingVisibility, string myPostsVisibility, 
                                  string connectionsPostsVisibility, string myRepostsVisibility, 
                                  string repostsOfMyConnectionsVisibility, 
                                  string organizationIAmCreatorVisibility, string organizationIFollowVisibility,
                                  string isOnlineVisibility, string deviceTypeVisibility, string lastActiveVisibility,
                                  string countryVisibility, string cityVisibility) 
        {
            StudentId = studentId;
            CreatedAtVisibility = createdAtVisibility;
            DateOfBirthVisibility = dateOfBirthVisibility;
            InterestedInEventsVisibility = interestedInEventsVisibility;
            SignedUpEventsVisibility = signedUpEventsVisibility;
            EducationVisibility = educationVisibility;
            WorkPositionVisibility = workPositionVisibility;
            LanguagesVisibility = languagesVisibility;
            InterestsVisibility = interestsVisibility;
            ContactEmailVisibility = contactEmailVisibility;
            PhoneNumberVisibility = phoneNumberVisibility;
            ProfileImageVisibility = profileImageVisibility;
            BannerImageVisibility = bannerImageVisibility;
            GalleryVisibility = galleryVisibility;

            ConnectionVisibility = connectionVisibility;
            FollowersVisibility = followersVisibility;
            FollowingVisibility = followingVisibility;
            MyPostsVisibility = myPostsVisibility;
            ConnectionsPostsVisibility = connectionsPostsVisibility;
            MyRepostsVisibility = myRepostsVisibility;
            RepostsOfMyConnectionsVisibility = repostsOfMyConnectionsVisibility;
            OrganizationIAmCreatorVisibility = organizationIAmCreatorVisibility;
            OrganizationIFollowVisibility = organizationIFollowVisibility;

            IsOnlineVisibility = isOnlineVisibility;
            DeviceTypeVisibility = deviceTypeVisibility;
            LastActiveVisibility = lastActiveVisibility;

            CountryVisibility = countryVisibility;
            CityVisibility = cityVisibility;

            PreferredLanguage = preferredLanguage;
            FrontendVersion = frontendVersion;
        }
    }
}
