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
                document.Education.Select(e => new Education(e.InstitutionName, e.Degree, e.StartDate, e.EndDate, e.Description)),
                document.Work.Select(w => new Work(w.Company, w.Position, w.StartDate, w.EndDate, w.Description)),
                document.Languages, // Directly use document.Languages
                document.Interests,
                document.IsTwoFactorEnabled,
                document.TwoFactorSecret,
                document.ContactEmail,
                document.PhoneNumber
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
                    InstitutionName = e.InstitutionName,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Description = e.Description
                }),
                Work = entity.Work.Select(w => new WorkDocument
                {
                    Company = w.Company,
                    Position = w.Position,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    Description = w.Description
                }),
                Languages = entity.Languages, // Directly use entity.Languages
                Interests = entity.Interests,
                IsTwoFactorEnabled = entity.IsTwoFactorEnabled,
                TwoFactorSecret = entity.TwoFactorSecret,
                ContactEmail = entity.ContactEmail,
                PhoneNumber = entity.PhoneNumber,
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
                    InstitutionName = e.InstitutionName,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Description = e.Description
                }),
                Work = document.Work.Select(w => new WorkDto
                {
                    Company = w.Company,
                    Position = w.Position,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    Description = w.Description
                }),
                Languages = document.Languages.Select(l => l.ToString()), // Convert Language to string
                Interests = document.Interests.Select(i => new InterestDto { Name = i.ToString() }),
                IsTwoFactorEnabled = document.IsTwoFactorEnabled,
                TwoFactorSecret = document.TwoFactorSecret,
                ContactEmail = document.ContactEmail,
                PhoneNumber = document.PhoneNumber,
            };

        public static UserNotifications AsEntity(this UserNotificationsDocument document)
            => new UserNotifications(
                document.StudentId,
                document.NotificationPreferences
            );

        public static UserNotificationsDocument AsDocument(this UserNotifications entity)
            => new UserNotificationsDocument
            {
                Id = Guid.NewGuid(), // Ensure a unique identifier is set
                StudentId = entity.StudentId,
                NotificationPreferences = entity.NotificationPreferences
            };

        public static NotificationPreferencesDto AsDto(this NotificationPreferences notificationPreferences)
            => new NotificationPreferencesDto
            {
                AccountChanges = notificationPreferences.AccountChanges,
                SystemLogin = notificationPreferences.SystemLogin,
                NewEvent = notificationPreferences.NewEvent,
                InterestBasedEvents = notificationPreferences.InterestBasedEvents,
                EventNotifications = notificationPreferences.EventNotifications,
                CommentsNotifications = notificationPreferences.CommentsNotifications,
                PostsNotifications = notificationPreferences.PostsNotifications,
                FriendsNotifications = notificationPreferences.FriendsNotifications
            };

        public static UserNotificationsDocument AsDocument(this NotificationPreferencesDto dto)
            => new UserNotificationsDocument
            {
                Id = Guid.NewGuid(),
                StudentId = dto.StudentId,
                NotificationPreferences = new NotificationPreferences(
                    dto.AccountChanges,
                    dto.SystemLogin,
                    dto.NewEvent,
                    dto.InterestBasedEvents,
                    dto.EventNotifications,
                    dto.CommentsNotifications,
                    dto.PostsNotifications,
                    dto.FriendsNotifications
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
                document.StudentId,
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
                    document.AvailableSettings.FrontendVersion,
                    document.AvailableSettings.PreferredLanguage
                )
            );

       public static UserSettingsDocument AsDocument(this UserSettings entity)
            => new UserSettingsDocument
            {
                Id = Guid.NewGuid(), // Ensure a unique identifier is set
                StudentId = entity.StudentId,
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
                    FrontendVersion = entity.AvailableSettings.FrontendVersion,
                    PreferredLanguage = entity.AvailableSettings.PreferredLanguage
                }
            };
    }
}
