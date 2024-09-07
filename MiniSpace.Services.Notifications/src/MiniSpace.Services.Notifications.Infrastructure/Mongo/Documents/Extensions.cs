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
                document.UpdatedAt,
                document.EventType,
                document.RelatedEntityId,
                document.Details);

        public static NotificationDocument AsDocument(this Notification entity)
            => new NotificationDocument
            {
                NotificationId = entity.NotificationId,
                UserId = entity.UserId,
                Message = entity.Message,
                Status = entity.Status.ToString(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                RelatedEntityId = entity.RelatedEntityId,
                EventType = entity.EventType,
                Details = entity.Details
            };

        public static NotificationDto AsDto(this Notification entity)
            => new NotificationDto
            {
                NotificationId = entity.NotificationId,
                UserId = entity.UserId,
                Message = entity.Message,
                Status = entity.Status.ToString(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                RelatedEntityId = entity.RelatedEntityId,
                EventType = entity.EventType,
                Details = entity.Details
            };

        public static NotificationDto AsDto(this NotificationDocument document)
            => new NotificationDto
            {
                NotificationId = document.NotificationId,
                UserId = document.UserId,
                Message = document.Message,
                Status = document.Status,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt,
                RelatedEntityId = document.RelatedEntityId,
                EventType = document.EventType,
                Details = document.Details
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

        public static UserNotifications AsEntity(this UserNotificationsDocument document)
        {
            var studentNotifications = new UserNotifications(document.UserId);
            foreach (var notificationDocument in document.Notifications)
            {
                var notification = notificationDocument.AsEntity(); 
                studentNotifications.AddNotification(notification);
            }
            return studentNotifications;
        }

        public static UserNotificationsDocument AsDocument(this UserNotifications entity)
        {
            var notifications = new List<NotificationDocument>();
            foreach (var notification in entity.Notifications) 
            {
                notifications.Add(notification.AsDocument()); 
            }

            return new UserNotificationsDocument
            {
                Id = Guid.NewGuid(), 
                UserId = entity.UserId,
                Notifications = notifications
            };
        }

        public static IEnumerable<NotificationDto> AsDto(this UserNotificationsDocument document)
        {
            return document.Notifications.Select(nd => nd.AsDto()); 
        }

        public static List<NotificationDocument> AsDocumentList(this IEnumerable<Notification> notifications)
        {
            return notifications.Select(n => n.AsDocument()).ToList();
        }
    }    
}
