namespace Astravent.Web.Wasm.DTO
{
    public class AvailableSettingsDto
    {
        public Visibility CreatedAtVisibility { get; set; }
        public Visibility DateOfBirthVisibility { get; set; }
        public Visibility InterestedInEventsVisibility { get; set; }
        public Visibility SignedUpEventsVisibility { get; set; }
        public Visibility EducationVisibility { get; set; }
        public Visibility WorkPositionVisibility { get; set; }
        public Visibility LanguagesVisibility { get; set; }
        public Visibility InterestsVisibility { get; set; }
        public Visibility ContactEmailVisibility { get; set; }
        public Visibility PhoneNumberVisibility { get; set; }
        public Visibility ProfileImageVisibility { get; set; }
        public Visibility BannerImageVisibility { get; set; }
        public Visibility GalleryVisibility { get; set; }

        public Visibility ConnectionVisibility { get; set; }
        public Visibility FollowersVisibility { get; set; }
        public Visibility FollowingVisibility { get; set; }
        public Visibility MyPostsVisibility { get; set; }
        public Visibility ConnectionsPostsVisibility { get; set; }
        public Visibility MyRepostsVisibility { get; set; }
        public Visibility RepostsOfMyConnectionsVisibility { get; set; }
        public Visibility OrganizationIAmCreatorVisibility { get; set; }
        public Visibility OrganizationIFollowVisibility { get; set; }

        public Visibility IsOnlineVisibility { get; set; }
        public Visibility DeviceTypeVisibility { get; set; }
        public Visibility LastActiveVisibility { get; set; }
        public Visibility CountryVisibility { get; set; }
        public Visibility CityVisibility { get; set; }

        public Visibility FriendListVisibility { get; set; }
        public Visibility FollowersListVisibility { get; set; }
        public Visibility FollowingListVisibility { get; set; }
        public Visibility MessageVisibility { get; set; }
        public Visibility ProfileVisibility { get; set; }
        public Visibility PostCommentVisibility { get; set; }
        public Visibility PostLikeVisibility { get; set; }
        public Visibility FriendRequestVisibility { get; set; }
        public Visibility TaggedPostVisibility { get; set; }
        public Visibility StoryVisibility { get; set; }
        public Visibility GroupMembershipVisibility { get; set; }

        public PreferredLanguage PreferredLanguage { get; set; }
        public FrontendVersion FrontendVersion { get; set; }
    }
}
