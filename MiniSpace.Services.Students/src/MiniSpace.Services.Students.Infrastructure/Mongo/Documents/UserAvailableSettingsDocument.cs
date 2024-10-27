using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class UserAvailableSettingsDocument
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
        public Visibility FriendListVisibility { get; set; }  // Who can see the user's friend list
        public Visibility FollowersListVisibility { get; set; }  // Who can see the user's followers list
        public Visibility FollowingListVisibility { get; set; }  // Who can see the user's following list
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

        public FrontendVersion FrontendVersion { get; set; }
        public PreferredLanguage PreferredLanguage { get; set; }

        public Visibility MessageVisibility { get; set; }  // Who can message the user
        public Visibility ProfileVisibility { get; set; }  // Overall profile visibility
        public Visibility PostCommentVisibility { get; set; }  // Who can comment on posts
        public Visibility PostLikeVisibility { get; set; }  // Who can like posts
        public Visibility FriendRequestVisibility { get; set; }  // Who can send friend requests
        public Visibility TaggedPostVisibility { get; set; }  // Who can see posts the user is tagged in
        public Visibility StoryVisibility { get; set; }  // Who can view stories
        public Visibility GroupMembershipVisibility { get; set; }  // Who can see user's group memberships
    }
}
