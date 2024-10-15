using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class AvailableSettingsDto
    {
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
    }
}
