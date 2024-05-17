using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Core.Entities;
using System;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Notification AsEntity(this NotificationDocument document)
            => new Notification(
                document.NotificationId,
                document.UserId,
                document.Message,
                Enum.Parse<NotificationStatus>(document.Status, true),
                document.CreatedAt,
                document.UpdatedAt);

        public static NotificationDocument AsDocument(this Notification entity)
            => new NotificationDocument
            {
                NotificationId = entity.NotificationId,
                UserId = entity.UserId,
                Message = entity.Message,
                Status = entity.Status.ToString(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

        public static NotificationDto AsDto(this NotificationDocument document)
            => new NotificationDto
            {
                NotificationId = document.NotificationId,
                UserId = document.UserId,
                Message = document.Message,
                Status = document.Status,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt
            };


        public static FriendEvent AsEntity(this FriendEventDocument document)
        {
            return new FriendEvent(
                document.Id,
                document.EventId,
                document.UserId,
                document.FriendId,
                document.EventType,
                document.Details,
                document.CreatedAt
            );
        }

        public static FriendEventDocument AsDocument(this FriendEvent entity)
        {
            return new FriendEventDocument
            {
                Id = entity.Id,
                EventId = entity.EventId,
                UserId = entity.UserId,
                FriendId = entity.FriendId, 
                EventType = entity.EventType,
                Details = entity.Details,
                CreatedAt = entity.CreatedAt
            };
        }

        public static FriendEventDto AsDto(this FriendEventDocument document)
        {
            return new FriendEventDto
            {
                Id = document.Id,
                EventId = document.EventId,
                UserId = document.UserId,
                EventType = document.EventType,
                Details = document.Details,
                CreatedAt = document.CreatedAt
            };
        }

       public static Student AsEntity(this StudentDocument document)
        {
            return new Student(
                document.Id,
                document.Email,
                document.FirstName,
                document.LastName,
                document.NumberOfFriends,
                document.ProfileImage,
                document.Description,
                document.DateOfBirth,
                document.EmailNotifications,
                document.IsBanned,
                document.IsOrganizer,
                document.State,
                document.CreatedAt
            );
        }

        public static StudentDocument AsDocument(this Student entity)
        {
            return new StudentDocument
            {
                Id = entity.Id,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                NumberOfFriends = entity.NumberOfFriends,
                ProfileImage = entity.ProfileImage,
                Description = entity.Description,
                DateOfBirth = entity.DateOfBirth,
                EmailNotifications = entity.EmailNotifications,
                IsBanned = entity.IsBanned,
                IsOrganizer = entity.IsOrganizer,
                State = entity.State,
                CreatedAt = entity.CreatedAt,
                InterestedInEvents = entity.InterestedInEvents,
                SignedUpEvents = entity.SignedUpEvents
            };
        }

        public static StudentDto AsDto(this StudentDocument document)
        {
            return new StudentDto
            {
                Id = document.Id,
                Email = document.Email,
                FirstName = document.FirstName,
                LastName = document.LastName,
                NumberOfFriends = document.NumberOfFriends,
                ProfileImage = document.ProfileImage,
                Description = document.Description,
                DateOfBirth = document.DateOfBirth,
                EmailNotifications = document.EmailNotifications,
                IsBanned = document.IsBanned,
                IsOrganizer = document.IsOrganizer,
                State = document.State,
                CreatedAt = document.CreatedAt,
                InterestedInEvents = document.InterestedInEvents,
                SignedUpEvents = document.SignedUpEvents
            };
        }

        public static StudentNotifications AsEntity(this StudentNotificationsDocument document)
        {
            var studentNotifications = new StudentNotifications(document.StudentId);
            foreach (var notificationDocument in document.Notifications)
            {
                var notification = notificationDocument.AsEntity(); 
                studentNotifications.AddNotification(notification);
            }
            return studentNotifications;
        }

        public static StudentNotificationsDocument AsDocument(this StudentNotifications entity)
        {
            var notifications = new List<NotificationDocument>();
            foreach (var notification in entity.Notifications) 
            {
                notifications.Add(notification.AsDocument()); 
            }

            return new StudentNotificationsDocument
            {
                Id = Guid.NewGuid(), 
                StudentId = entity.StudentId,
                Notifications = notifications
            };
        }

        public static IEnumerable<NotificationDto> AsDto(this StudentNotificationsDocument document)
        {
            return document.Notifications.Select(nd => nd.AsDto()); 
        }

        public static List<NotificationDocument> AsDocumentList(this IEnumerable<Notification> notifications)
        {
            return notifications.Select(n => n.AsDocument()).ToList();
        }
    }    
}
