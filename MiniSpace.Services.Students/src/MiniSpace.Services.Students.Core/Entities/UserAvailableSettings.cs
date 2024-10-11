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
        public Visibility MyPostsVisibility { get; private set; }
        public Visibility ConnectionsPostsVisibility { get; private set; }
        public Visibility MyRepostsVisibility { get; private set; }
        public Visibility RepostsOfMyConnectionsVisibility { get; private set; }

        public Visibility OrganizationIAmCreatorVisibility { get; private set; }
        public Visibility OrganizationIFollowVisibility { get; private set; }

        public FrontendVersion FrontendVersion { get; private set; }
        public PreferredLanguage PreferredLanguage { get; private set; }

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
            MyPostsVisibility = Visibility.Everyone;
            ConnectionsPostsVisibility = Visibility.Everyone;
            MyRepostsVisibility = Visibility.Everyone;
            RepostsOfMyConnectionsVisibility = Visibility.Everyone;
            OrganizationIAmCreatorVisibility = Visibility.Everyone;
            OrganizationIFollowVisibility = Visibility.Everyone;

            FrontendVersion = FrontendVersion.Default;
            PreferredLanguage = PreferredLanguage.English;
        }

        public UserAvailableSettings(Visibility createdAtVisibility, Visibility dateOfBirthVisibility, Visibility interestedInEventsVisibility,
                                     Visibility signedUpEventsVisibility, Visibility educationVisibility, Visibility workPositionVisibility,
                                     Visibility languagesVisibility, Visibility interestsVisibility, Visibility contactEmailVisibility,
                                     Visibility phoneNumberVisibility, Visibility profileImageVisibility, Visibility bannerImageVisibility,
                                     Visibility galleryVisibility, FrontendVersion frontendVersion, PreferredLanguage preferredLanguage,
                                     Visibility connectionVisibility, Visibility followersVisibility, Visibility followingVisibility,
                                     Visibility myPostsVisibility, Visibility connectionsPostsVisibility, Visibility myRepostsVisibility,
                                     Visibility repostsOfMyConnectionsVisibility, Visibility organizationIAmCreatorVisibility,
                                     Visibility organizationIFollowVisibility)
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
            MyPostsVisibility = myPostsVisibility;
            ConnectionsPostsVisibility = connectionsPostsVisibility;
            MyRepostsVisibility = myRepostsVisibility;
            RepostsOfMyConnectionsVisibility = repostsOfMyConnectionsVisibility;
            OrganizationIAmCreatorVisibility = organizationIAmCreatorVisibility;
            OrganizationIFollowVisibility = organizationIFollowVisibility;

            FrontendVersion = frontendVersion;
            PreferredLanguage = preferredLanguage;
        }

        public void UpdateSettings(Visibility createdAtVisibility, Visibility dateOfBirthVisibility, Visibility interestedInEventsVisibility,
                                   Visibility signedUpEventsVisibility, Visibility educationVisibility, Visibility workPositionVisibility,
                                   Visibility languagesVisibility, Visibility interestsVisibility, Visibility contactEmailVisibility,
                                   Visibility phoneNumberVisibility, Visibility profileImageVisibility, Visibility bannerImageVisibility,
                                   Visibility galleryVisibility, FrontendVersion frontendVersion, PreferredLanguage preferredLanguage,
                                   Visibility connectionVisibility, Visibility followersVisibility, Visibility followingVisibility,
                                   Visibility myPostsVisibility, Visibility connectionsPostsVisibility, Visibility myRepostsVisibility,
                                   Visibility repostsOfMyConnectionsVisibility, Visibility organizationIAmCreatorVisibility,
                                   Visibility organizationIFollowVisibility)
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
            MyPostsVisibility = myPostsVisibility;
            ConnectionsPostsVisibility = connectionsPostsVisibility;
            MyRepostsVisibility = myRepostsVisibility;
            RepostsOfMyConnectionsVisibility = repostsOfMyConnectionsVisibility;
            OrganizationIAmCreatorVisibility = organizationIAmCreatorVisibility;
            OrganizationIFollowVisibility = organizationIFollowVisibility;

            FrontendVersion = frontendVersion;
            PreferredLanguage = preferredLanguage;
        }
    }
}
