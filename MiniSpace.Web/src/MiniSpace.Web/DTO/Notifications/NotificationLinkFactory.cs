using MiniSpace.Web.DTO.Enums;
using MiniSpace.Web.DTO.Notifications;
using System;

namespace MiniSpace.Web.DTO.Notifications
{
    public static class NotificationLinkFactory
    {
        public static string GetNotificationLink(NotificationDto notification)
        {
            switch (notification.EventType)
            {
                case NotificationEventType.NewEvent:
                case NotificationEventType.EventDeleted:
                case NotificationEventType.EventNewSignUp:
                case NotificationEventType.EventNewSignUpFriend:
                case NotificationEventType.StudentCancelledSignedUpToEvent:
                case NotificationEventType.StudentShowedInterestInEvent:
                case NotificationEventType.StudentCancelledInterestInEvent:
                case NotificationEventType.EventParticipantAdded:
                case NotificationEventType.EventParticipantRemoved:
                case NotificationEventType.EventReminder:
                    return $"/events/{notification.RelatedEntityId}";
                
                case NotificationEventType.PostCreated:
                case NotificationEventType.PostUpdated:
                case NotificationEventType.MentionedInPost:
                    // return $"/events/{notification.ParentEntityId}/posts/{notification.RelatedEntityId}/author/{Uri.EscapeDataString(notification.AdditionalInfo)}";
                
                case NotificationEventType.CommentCreated:
                case NotificationEventType.CommentUpdated:
                    // return $"/events/{notification.ParentEntityId}/posts/{notification.GrandParentEntityId}/comments/{notification.RelatedEntityId}";
                
                case NotificationEventType.ReactionAdded:
                case NotificationEventType.ReportCreated:
                case NotificationEventType.ReportDeleted:
                case NotificationEventType.ReportRejected:
                case NotificationEventType.ReportResolved:
                case NotificationEventType.ReportReviewStarted:
                    // return $"/reports/{notification.RelatedEntityId}";

                default:
                    return $"/student-details/{notification.RelatedEntityId}";
            }
        }
    }
}
