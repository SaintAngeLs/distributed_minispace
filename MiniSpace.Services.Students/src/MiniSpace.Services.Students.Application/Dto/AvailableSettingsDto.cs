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
        public string FriendListVisibility { get; set; }  // Who can see the user's friend list
        public string FollowersListVisibility { get; set; }  // Who can see the user's followers list
        public string FollowingListVisibility { get; set; }  // Who can see the user's following list
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

        public string MessageVisibility { get; set; }  // Who can message the user
        public string ProfileVisibility { get; set; }  // Overall profile visibility
        public string PostCommentVisibility { get; set; }  // Who can comment on posts
        public string PostLikeVisibility { get; set; }  // Who can like posts
        public string FriendRequestVisibility { get; set; }  // Who can send friend requests
        public string TaggedPostVisibility { get; set; }  // Who can see posts the user is tagged in
        public string StoryVisibility { get; set; }  // Who can view stories
        public string GroupMembershipVisibility { get; set; }  // Who can see the user's group memberships
    }
}
