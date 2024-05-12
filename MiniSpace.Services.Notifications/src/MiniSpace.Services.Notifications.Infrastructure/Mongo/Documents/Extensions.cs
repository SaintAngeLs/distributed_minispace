using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Core.Entities;
using System;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        // Converts a NotificationDocument to its domain entity equivalent
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
    }    
}
