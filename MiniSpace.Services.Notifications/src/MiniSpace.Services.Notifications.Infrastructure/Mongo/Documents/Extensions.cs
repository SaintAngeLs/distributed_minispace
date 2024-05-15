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
    }    
}
