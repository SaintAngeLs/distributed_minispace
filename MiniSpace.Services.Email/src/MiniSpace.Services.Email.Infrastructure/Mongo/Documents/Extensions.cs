using MiniSpace.Services.Email.Application.Dto;
using MiniSpace.Services.Email.Core.Entities;
using MiniSpace.Services.Email.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Email.Infrastructure.Mongo.Extensions
{
    public static class EmailExtensions
    {
        public static EmailNotificationDocument AsDocument(this EmailNotification entity)
        {
            return new EmailNotificationDocument
            {
                EmailNotificationId = entity.EmailNotificationId,
                UserId = entity.UserId,
                EmailAddress = entity.EmailAddress,
                Subject = entity.Subject,
                Body = entity.Body,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                SentAt = entity.SentAt
            };
        }

        public static EmailNotification AsEntity(this EmailNotificationDocument document)
        {
            return new EmailNotification(
                document.EmailNotificationId,
                document.UserId,
                document.EmailAddress,
                document.Subject,
                document.Body,
                document.Status
            )
            {
                CreatedAt = document.CreatedAt,
                SentAt = document.SentAt
            };
        }
        public static StudentEmailsDocument AsDocument(this StudentEmails entity)
        {
            return new StudentEmailsDocument
            {
                Id = entity.StudentId, 
                StudentId = entity.StudentId,
                EmailNotifications = entity.EmailNotifications.Select(en => en.AsDocument()).ToList()
            };
        }

        public static StudentEmails AsEntity(this StudentEmailsDocument document)
        {
            var studentEmails = new StudentEmails(document.StudentId);
            foreach (var notificationDocument in document.EmailNotifications)
            {
                studentEmails.AddEmailNotification(notificationDocument.AsEntity());
            }
            return studentEmails;
        }

         public static EmailNotificationDto AsDto(this EmailNotification entity)
        {
            return new EmailNotificationDto
            {
                EmailNotificationId = entity.EmailNotificationId,
                UserId = entity.UserId,
                EmailAddress = entity.EmailAddress,
                Subject = entity.Subject,
                Body = entity.Body,
                Status = entity.Status.ToString(),
                CreatedAt = entity.CreatedAt,
                SentAt = entity.SentAt
            };
        }

        public static EmailNotification AsEntity(this EmailNotificationDto dto)
        {
            return new EmailNotification(
                dto.EmailNotificationId,
                dto.UserId,
                dto.EmailAddress,
                dto.Subject,
                dto.Body,
                Enum.Parse<EmailNotificationStatus>(dto.Status, true)
            )
            {
                CreatedAt = dto.CreatedAt,
                SentAt = dto.SentAt
            };
        }
        

        // public static StudentEmailsDto AsDto(this StudentEmails entity)
        // {
        //     var dto = new StudentEmailsDto(entity.StudentId);
        //     foreach (var notification in entity.EmailNotifications)
        //     {
        //         dto.AddNotification(notification.AsDto());
        //     }
        //     return dto;
        // }

        // public static StudentEmails AsEntity(this StudentEmailsDto dto)
        // {
        //     var studentEmails = new StudentEmails(dto.StudentId);
        //     foreach (var notificationDto in dto.Notifications)
        //     {
        //         studentEmails.AddEmailNotification(notificationDto.AsEntity());
        //     }
        //     return studentEmails;
        // }
    }
}
