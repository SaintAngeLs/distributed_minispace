using System;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class UserAvailableSettings
    {
        public Visibility CreatedAtVisibility { get; private set; }
        public Visibility DateOfBirthVisibility { get; private set; }
        public Visibility InterestedInEventsVisibility { get; private set; }
        public Visibility SignedUpEventsVisibility { get; private set; }
        public Visibility EducationVisibility { get; private set; }
        public Visibility WorkPositionVisibility { get; private set; }
        public Visibility LanguagesVisibility { get; private set; }
        public Visibility InterestsVisibility { get; private set; }
        public Visibility ContactEmailVisibility { get; private set; }
        public Visibility PhoneNumberVisibility { get; private set; }
        public Visibility ProfileImageVisibility { get; private set; }
        public Visibility BannerImageVisibility { get; private set; }
        public Visibility GalleryVisibility { get; private set; }

        public Visibility ConnectionVisibility { get; private set; }
        public Visibility FollowersVisibility { get; private set; }
        public Visibility FollowingVisibility { get; private set; }
        public Visibility FriendListVisibility { get; private set; }  // Who can see the user's friend list
        public Visibility FollowersListVisibility { get; private set; }  // Who can see the user's followers list
        public Visibility FollowingListVisibility { get; private set; }  // Who can see the user's following list
        public Visibility MyPostsVisibility { get; private set; }
        public Visibility ConnectionsPostsVisibility { get; private set; }
        public Visibility MyRepostsVisibility { get; private set; }
        public Visibility RepostsOfMyConnectionsVisibility { get; private set; }

        public Visibility OrganizationIAmCreatorVisibility { get; private set; }
        public Visibility OrganizationIFollowVisibility { get; private set; }

        public Visibility IsOnlineVisibility { get; private set; }
        public Visibility DeviceTypeVisibility { get; private set; }
        public Visibility LastActiveVisibility { get; private set; }

        public Visibility CountryVisibility { get; private set; }
        public Visibility CityVisibility { get; private set; }

        public FrontendVersion FrontendVersion { get; private set; }
        public PreferredLanguage PreferredLanguage { get; private set; }

        public Visibility MessageVisibility { get; private set; }  // Who can message the user
        public Visibility ProfileVisibility { get; private set; }  // Overall profile visibility
        public Visibility PostCommentVisibility { get; private set; }  // Who can comment on posts
        public Visibility PostLikeVisibility { get; private set; }  // Who can like posts
        public Visibility FriendRequestVisibility { get; private set; }  // Who can send friend requests
        public Visibility TaggedPostVisibility { get; private set; }  // Who can see posts the user is tagged in
        public Visibility StoryVisibility { get; private set; }  // Who can view stories
        public Visibility GroupMembershipVisibility { get; private set; }  // Who can see user's group memberships
        public bool BlockedUsersVisibility { get; private set; }  // Control visibility settings around blocked users

        public UserAvailableSettings()
        {
            CreatedAtVisibility = Visibility.Everyone;
            DateOfBirthVisibility = Visibility.Everyone;
            InterestedInEventsVisibility = Visibility.Everyone;
            SignedUpEventsVisibility = Visibility.Everyone;
            EducationVisibility = Visibility.Everyone;
            WorkPositionVisibility = Visibility.Everyone;
            LanguagesVisibility = Visibility.Everyone;
            InterestsVisibility = Visibility.Everyone;
            ContactEmailVisibility = Visibility.Everyone;
            PhoneNumberVisibility = Visibility.Everyone;
            ProfileImageVisibility = Visibility.Everyone;
            BannerImageVisibility = Visibility.Everyone;
            GalleryVisibility = Visibility.Everyone;

            ConnectionVisibility = Visibility.Everyone;
            FollowersVisibility = Visibility.Everyone;
            FollowingVisibility = Visibility.Everyone;
            FriendListVisibility = Visibility.Everyone;  // Default friend list visibility
            FollowersListVisibility = Visibility.Everyone;  // Default followers list visibility
            FollowingListVisibility = Visibility.Everyone;  // Default following list visibility
            MyPostsVisibility = Visibility.Everyone;
            ConnectionsPostsVisibility = Visibility.Everyone;
            MyRepostsVisibility = Visibility.Everyone;
            RepostsOfMyConnectionsVisibility = Visibility.Everyone;
            OrganizationIAmCreatorVisibility = Visibility.Everyone;
            OrganizationIFollowVisibility = Visibility.Everyone;

            IsOnlineVisibility = Visibility.Everyone;
            DeviceTypeVisibility = Visibility.Everyone;
            LastActiveVisibility = Visibility.Everyone;

            CountryVisibility = Visibility.Everyone;
            CityVisibility = Visibility.Everyone;

            FrontendVersion = FrontendVersion.Default;
            PreferredLanguage = PreferredLanguage.English;

            MessageVisibility = Visibility.Everyone;
            ProfileVisibility = Visibility.Everyone;
            PostCommentVisibility = Visibility.Everyone;
            PostLikeVisibility = Visibility.Everyone;
            FriendRequestVisibility = Visibility.Everyone;
            TaggedPostVisibility = Visibility.Everyone;
            StoryVisibility = Visibility.Everyone;
            GroupMembershipVisibility = Visibility.Everyone;
            BlockedUsersVisibility = false;  // Default blocked users visibility
        }

        public UserAvailableSettings(Visibility createdAtVisibility, Visibility dateOfBirthVisibility, Visibility interestedInEventsVisibility,
                                     Visibility signedUpEventsVisibility, Visibility educationVisibility, Visibility workPositionVisibility,
                                     Visibility languagesVisibility, Visibility interestsVisibility, Visibility contactEmailVisibility,
                                     Visibility phoneNumberVisibility, Visibility profileImageVisibility, Visibility bannerImageVisibility,
                                     Visibility galleryVisibility, FrontendVersion frontendVersion, PreferredLanguage preferredLanguage,
                                     Visibility connectionVisibility, Visibility followersVisibility, Visibility followingVisibility,
                                     Visibility friendListVisibility, Visibility followersListVisibility, Visibility followingListVisibility,
                                     Visibility myPostsVisibility, Visibility connectionsPostsVisibility, Visibility myRepostsVisibility,
                                     Visibility repostsOfMyConnectionsVisibility, Visibility organizationIAmCreatorVisibility,
                                     Visibility organizationIFollowVisibility, Visibility isOnlineVisibility, Visibility deviceTypeVisibility,
                                     Visibility lastActiveVisibility, Visibility countryVisibility, Visibility cityVisibility)
        {
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
            FriendListVisibility = friendListVisibility;
            FollowersListVisibility = followersListVisibility;
            FollowingListVisibility = followingListVisibility;
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

            FrontendVersion = frontendVersion;
            PreferredLanguage = preferredLanguage;

            MessageVisibility = Visibility.Everyone;
            ProfileVisibility = Visibility.Everyone;
            PostCommentVisibility = Visibility.Everyone;
            PostLikeVisibility = Visibility.Everyone;
            FriendRequestVisibility = Visibility.Everyone;
            TaggedPostVisibility = Visibility.Everyone;
            StoryVisibility = Visibility.Everyone;
            GroupMembershipVisibility = Visibility.Everyone;
            BlockedUsersVisibility = false;  // Default blocked users visibility
        }

        public void UpdateSettings(Visibility createdAtVisibility, Visibility dateOfBirthVisibility, Visibility interestedInEventsVisibility,
                                   Visibility signedUpEventsVisibility, Visibility educationVisibility, Visibility workPositionVisibility,
                                   Visibility languagesVisibility, Visibility interestsVisibility, Visibility contactEmailVisibility,
                                   Visibility phoneNumberVisibility, Visibility profileImageVisibility, Visibility bannerImageVisibility,
                                   Visibility galleryVisibility, FrontendVersion frontendVersion, PreferredLanguage preferredLanguage,
                                   Visibility connectionVisibility, Visibility followersVisibility, Visibility followingVisibility,
                                   Visibility friendListVisibility, Visibility followersListVisibility, Visibility followingListVisibility,
                                   Visibility myPostsVisibility, Visibility connectionsPostsVisibility, Visibility myRepostsVisibility,
                                   Visibility repostsOfMyConnectionsVisibility, Visibility organizationIAmCreatorVisibility,
                                   Visibility organizationIFollowVisibility, Visibility isOnlineVisibility, Visibility deviceTypeVisibility,
                                   Visibility lastActiveVisibility, Visibility countryVisibility, Visibility cityVisibility)
        {
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
            FriendListVisibility = friendListVisibility;
            FollowersListVisibility = followersListVisibility;
            FollowingListVisibility = followingListVisibility;
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

            FrontendVersion = frontendVersion;
            PreferredLanguage = preferredLanguage;
        }
    }
}
