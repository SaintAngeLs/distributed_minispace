using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Linq;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Student AsEntity(this StudentDocument document)
            => new Student(
                document.Id,
                document.Email,
                document.CreatedAt,
                document.FirstName,
                document.LastName,
                document.ProfileImageUrl,
                document.Description,
                document.DateOfBirth,
                document.EmailNotifications,
                document.IsBanned,
                document.State,
                document.InterestedInEvents,
                document.SignedUpEvents,
                document.BannerUrl,
                document.Education.Select(e => new Education(e.OrganizationId, e.InstitutionName, e.Degree, e.StartDate, e.EndDate, e.Description)),
                document.Work.Select(w => new Work(w.OrganizationId, w.Company, w.Position, w.StartDate, w.EndDate, w.Description)),
                document.Languages.Select(l => Enum.Parse<Language>(l.ToString())).ToList(), 
                document.Interests.Select(i => Enum.Parse<Interest>(i.ToString())).ToList(),
                document.IsTwoFactorEnabled,
                document.TwoFactorSecret,
                document.ContactEmail,
                document.PhoneNumber,
                document.Country,
                document.City,
                document.IsOnline,
                document.DeviceType,
                document.LastActive  
            );

        public static StudentDocument AsDocument(this Student entity)
            => new StudentDocument()
            {
                Id = entity.Id,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                ProfileImageUrl = entity.ProfileImageUrl,
                Description = entity.Description,
                DateOfBirth = entity.DateOfBirth,
                EmailNotifications = entity.EmailNotifications,
                IsBanned = entity.IsBanned,
                State = entity.State,
                CreatedAt = entity.CreatedAt,
                InterestedInEvents = entity.InterestedInEvents,
                SignedUpEvents = entity.SignedUpEvents,
                BannerUrl = entity.BannerUrl,
                Education = entity.Education.Select(e => new EducationDocument
                {
                    OrganizationId = e.OrganizationId,
                    InstitutionName = e.InstitutionName,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Description = e.Description
                }),
                Work = entity.Work.Select(w => new WorkDocument
                {
                    OrganizationId = w.OrganizationId,
                    Company = w.Company,
                    Position = w.Position,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    Description = w.Description
                }),
                Languages = entity.Languages.Select(l => l.ToString()),
                Interests = entity.Interests.Select(i => i.ToString()), 
                IsTwoFactorEnabled = entity.IsTwoFactorEnabled,
                TwoFactorSecret = entity.TwoFactorSecret,
                ContactEmail = entity.ContactEmail,
                PhoneNumber = entity.PhoneNumber,
                Country = entity.Country,
                City = entity.City,
                IsOnline = entity.IsOnline,
                DeviceType = entity.DeviceType,
                LastActive = entity.LastActive
            };

        public static StudentDto AsDto(this StudentDocument document)
            => new StudentDto()
            {
                Id = document.Id,
                Email = document.Email,
                FirstName = document.FirstName,
                LastName = document.LastName,
                ProfileImageUrl = document.ProfileImageUrl,
                Description = document.Description,
                DateOfBirth = document.DateOfBirth,
                EmailNotifications = document.EmailNotifications,
                IsBanned = document.IsBanned,
                State = document.State.ToString().ToLowerInvariant(),
                CreatedAt = document.CreatedAt,
                InterestedInEvents = document.InterestedInEvents,
                SignedUpEvents = document.SignedUpEvents,
                BannerUrl = document.BannerUrl,
                Education = document.Education.Select(e => new EducationDto
                {
                    OrganizationId = e.OrganizationId,
                    InstitutionName = e.InstitutionName,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Description = e.Description
                }),
                Work = document.Work.Select(w => new WorkDto
                {
                    OrganizationId = w.OrganizationId,
                    Company = w.Company,
                    Position = w.Position,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    Description = w.Description
                }),
                Languages = document.Languages,
                Interests = document.Interests,
                IsTwoFactorEnabled = document.IsTwoFactorEnabled,
                TwoFactorSecret = document.TwoFactorSecret,
                ContactEmail = document.ContactEmail,
                PhoneNumber = document.PhoneNumber,
                Country = document.Country,
                City = document.City,
                IsOnline = document.IsOnline,
                DeviceType = document.DeviceType,
                LastActive = document.LastActive
            };

        public static UserNotifications AsEntity(this UserNotificationsDocument document)
            => new UserNotifications(
                document.UserId,
                document.NotificationPreferences
            );

        public static UserNotificationsDocument AsDocument(this UserNotifications entity)
            => new UserNotificationsDocument
            {
                Id = Guid.NewGuid(), 
                UserId = entity.UserId,
                NotificationPreferences = entity.NotificationPreferences
            };

        public static NotificationPreferencesDto AsDto(this NotificationPreferences notificationPreferences)
        => new NotificationPreferencesDto
        {
            SystemLogin = notificationPreferences.SystemLogin,
            InterestBasedEvents = notificationPreferences.InterestBasedEvents,
            EventNotifications = notificationPreferences.EventNotifications,
            CommentsNotifications = notificationPreferences.CommentsNotifications,
            PostsNotifications = notificationPreferences.PostsNotifications,
            EventRecommendation = notificationPreferences.EventRecommendation,
            FriendsRecommendation = notificationPreferences.FriendsRecommendation,
            FriendsPosts = notificationPreferences.FriendsPosts,
            PostsRecommendation = notificationPreferences.PostsRecommendation,
            EventsIAmInterestedInNotification = notificationPreferences.EventsIAmInterestedInNotification,
            EventsIAmSignedUpToNotification = notificationPreferences.EventsIAmSignedUpToNotification,
            PostsOfPeopleIFollowNotification = notificationPreferences.PostsOfPeopleIFollowNotification,
            EventNotificationForPeopleIFollow = notificationPreferences.EventNotificationForPeopleIFollow,

            NewFriendsRequests = notificationPreferences.NewFriendsRequests,
            MyRequestsAccepted = notificationPreferences.MyRequestsAccepted,
            FriendsPostsNotifications = notificationPreferences.FriendsPostsNotifications
        };

        public static UserNotificationsDocument AsDocument(this NotificationPreferencesDto dto)
            => new UserNotificationsDocument
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                NotificationPreferences = new NotificationPreferences(
                    dto.SystemLogin,
                    dto.InterestBasedEvents,
                    dto.EventNotifications,
                    dto.CommentsNotifications,
                    dto.PostsNotifications,
                    dto.EventRecommendation,
                    dto.FriendsRecommendation,
                    dto.FriendsPosts,
                    dto.PostsRecommendation,
                    dto.EventsIAmInterestedInNotification,
                    dto.EventsIAmSignedUpToNotification,
                    dto.PostsOfPeopleIFollowNotification,
                    dto.EventNotificationForPeopleIFollow,

                    dto.NewFriendsRequests,
                    dto.MyRequestsAccepted,
                    dto.FriendsPostsNotifications
                )
            };

        public static UserGallery AsEntity(this UserGalleryDocument document)
        {
            var gallery = new UserGallery(document.UserId);
            foreach (var image in document.GalleryOfImages)
            {
                gallery.AddGalleryImage(image.ImageId, image.ImageUrl);
            }
            return gallery;
        }

        public static UserGalleryDocument AsDocument(this UserGallery entity)
            => new UserGalleryDocument
            {
                Id = Guid.NewGuid(), 
                UserId = entity.UserId,
                GalleryOfImages = entity.GalleryOfImages.Select(gi => new GalleryImageDocument(gi.ImageId, gi.ImageUrl) { DateAdded = gi.DateAdded })
            };

        public static UserGalleryDto AsDto(this UserGalleryDocument document)
            => new UserGalleryDto(document.UserId, document.GalleryOfImages.Select(gi => new GalleryImageDto(gi.ImageId, gi.ImageUrl, gi.DateAdded)));

        public static UserSettings AsEntity(this UserSettingsDocument document)
            => new UserSettings(
                document.UserId,
                new UserAvailableSettings(
                    document.AvailableSettings.CreatedAtVisibility,
                    document.AvailableSettings.DateOfBirthVisibility,
                    document.AvailableSettings.InterestedInEventsVisibility,
                    document.AvailableSettings.SignedUpEventsVisibility,
                    document.AvailableSettings.EducationVisibility,
                    document.AvailableSettings.WorkPositionVisibility,
                    document.AvailableSettings.LanguagesVisibility,
                    document.AvailableSettings.InterestsVisibility,
                    document.AvailableSettings.ContactEmailVisibility,
                    document.AvailableSettings.PhoneNumberVisibility,
                    document.AvailableSettings.ProfileImageVisibility,  
                    document.AvailableSettings.BannerImageVisibility, 
                    document.AvailableSettings.GalleryVisibility,  
                    document.AvailableSettings.FrontendVersion,
                    document.AvailableSettings.PreferredLanguage,
                    
                    document.AvailableSettings.ConnectionVisibility,
                    document.AvailableSettings.FollowersVisibility,
                    document.AvailableSettings.FollowingVisibility,
                    document.AvailableSettings.MyPostsVisibility,
                    document.AvailableSettings.ConnectionsPostsVisibility,
                    document.AvailableSettings.MyRepostsVisibility,
                    document.AvailableSettings.RepostsOfMyConnectionsVisibility,
                    document.AvailableSettings.OrganizationIAmCreatorVisibility,
                    document.AvailableSettings.OrganizationIFollowVisibility
                )
            );

       public static UserSettingsDocument AsDocument(this UserSettings entity)
            => new UserSettingsDocument
            {
                Id = Guid.NewGuid(), 
                UserId = entity.UserId,
                AvailableSettings = new UserAvailableSettingsDocument
                {
                    CreatedAtVisibility = entity.AvailableSettings.CreatedAtVisibility,
                    DateOfBirthVisibility = entity.AvailableSettings.DateOfBirthVisibility,
                    InterestedInEventsVisibility = entity.AvailableSettings.InterestedInEventsVisibility,
                    SignedUpEventsVisibility = entity.AvailableSettings.SignedUpEventsVisibility,
                    EducationVisibility = entity.AvailableSettings.EducationVisibility,
                    WorkPositionVisibility = entity.AvailableSettings.WorkPositionVisibility,
                    LanguagesVisibility = entity.AvailableSettings.LanguagesVisibility,
                    InterestsVisibility = entity.AvailableSettings.InterestsVisibility,
                    ContactEmailVisibility = entity.AvailableSettings.ContactEmailVisibility,
                    PhoneNumberVisibility = entity.AvailableSettings.PhoneNumberVisibility,
                    ProfileImageVisibility = entity.AvailableSettings.ProfileImageVisibility, 
                    BannerImageVisibility = entity.AvailableSettings.BannerImageVisibility,   
                    GalleryVisibility = entity.AvailableSettings.GalleryVisibility,           

                    ConnectionVisibility = entity.AvailableSettings.ConnectionVisibility,
                    FollowersVisibility = entity.AvailableSettings.FollowersVisibility,
                    FollowingVisibility = entity.AvailableSettings.FollowingVisibility,
                    MyPostsVisibility = entity.AvailableSettings.MyPostsVisibility,
                    ConnectionsPostsVisibility = entity.AvailableSettings.ConnectionsPostsVisibility,
                    MyRepostsVisibility = entity.AvailableSettings.MyRepostsVisibility,
                    RepostsOfMyConnectionsVisibility = entity.AvailableSettings.RepostsOfMyConnectionsVisibility,
                    OrganizationIAmCreatorVisibility = entity.AvailableSettings.OrganizationIAmCreatorVisibility,
                    OrganizationIFollowVisibility = entity.AvailableSettings.OrganizationIFollowVisibility,

                    FrontendVersion = entity.AvailableSettings.FrontendVersion,
                    PreferredLanguage = entity.AvailableSettings.PreferredLanguage
                }
            };
    }
}
