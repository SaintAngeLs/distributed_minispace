using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
     public static class Extensions
    {
        public static Student AsEntity(this StudentDocument document)
            => new Student(
                document.Id,
                document.Email,
                document.CreatedAt,
                document.FirstName,
                document.LastName,
                document.NumberOfFriends,
                document.ProfileImageUrl,
                document.Description,
                document.DateOfBirth,
                document.EmailNotifications,
                document.IsBanned,
                document.IsOrganizer,
                document.State,
                document.InterestedInEvents,
                document.SignedUpEvents,
                document.BannerUrl,
                document.GalleryOfImageUrls,
                document.Education,
                document.WorkPosition,
                document.Company,
                document.Languages,
                document.Interests,
                document.IsTwoFactorEnabled,
                document.TwoFactorSecret,
                document.ContactEmail 
            );

        public static StudentDocument AsDocument(this Student entity)
            => new StudentDocument()
            {
                Id = entity.Id,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                NumberOfFriends = entity.NumberOfFriends,
                ProfileImageUrl = entity.ProfileImageUrl,
                Description = entity.Description,
                DateOfBirth = entity.DateOfBirth,
                EmailNotifications = entity.EmailNotifications,
                IsBanned = entity.IsBanned,
                IsOrganizer = entity.IsOrganizer,
                State = entity.State,
                CreatedAt = entity.CreatedAt,
                InterestedInEvents = entity.InterestedInEvents,
                SignedUpEvents = entity.SignedUpEvents,
                BannerUrl = entity.BannerUrl,
                GalleryOfImageUrls = entity.GalleryOfImageUrls,
                Education = entity.Education,
                WorkPosition = entity.WorkPosition,
                Company = entity.Company,
                Languages = entity.Languages,
                Interests = entity.Interests,
                IsTwoFactorEnabled = entity.IsTwoFactorEnabled,
                TwoFactorSecret = entity.TwoFactorSecret,
                ContactEmail = entity.ContactEmail
            };

        public static StudentDto AsDto(this StudentDocument document)
            => new StudentDto()
            {
                Id = document.Id,
                Email = document.Email,
                FirstName = document.FirstName,
                LastName = document.LastName,
                NumberOfFriends = document.NumberOfFriends,
                ProfileImageUrl = document.ProfileImageUrl,
                Description = document.Description,
                DateOfBirth = document.DateOfBirth,
                EmailNotifications = document.EmailNotifications,
                IsBanned = document.IsBanned,
                IsOrganizer = document.IsOrganizer,
                State = document.State.ToString().ToLowerInvariant(),
                CreatedAt = document.CreatedAt,
                InterestedInEvents = document.InterestedInEvents,
                SignedUpEvents = document.SignedUpEvents,
                BannerUrl = document.BannerUrl,
                GalleryOfImageUrls = document.GalleryOfImageUrls,
                Education = document.Education,
                WorkPosition = document.WorkPosition,
                Company = document.Company,
                Languages = document.Languages,
                Interests = document.Interests,
                IsTwoFactorEnabled = document.IsTwoFactorEnabled,
                TwoFactorSecret = document.TwoFactorSecret,
                ContactEmail = document.ContactEmail 
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
    }
}
