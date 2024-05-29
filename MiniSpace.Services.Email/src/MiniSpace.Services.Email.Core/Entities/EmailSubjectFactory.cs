namespace MiniSpace.Services.Email.Core.Entities
{
    public class EmailSubjectFactory
    {
        private static readonly Dictionary<NotificationEventType, IEmailSubjectStrategy> Strategies =
            new Dictionary<NotificationEventType, IEmailSubjectStrategy>
            {
                { NotificationEventType.NewFriendRequest, new NewFriendRequestSubject() },
                { NotificationEventType.FriendRequestAccepted, new FriendRequestAcceptedSubject() },
                { NotificationEventType.NewEvent, new NewEventSubject() },
                { NotificationEventType.EventDeleted, new EventDeletedSubject() },
                { NotificationEventType.PostUpdated, new PostUpdatedSubject() },
                { NotificationEventType.EventNewSignUp, new EventNewSignUpSubject() },
                { NotificationEventType.EventNewSignUpFriend, new EventNewSignUpFriendSubject() },
                { NotificationEventType.StudentCancelledSignedUpToEvent, new StudentCancelledSignedUpToEventSubject() },
                { NotificationEventType.StudentShowedInterestInEvent, new StudentShowedInterestInEventSubject() },
                { NotificationEventType.StudentCancelledInterestInEvent, new StudentCancelledInterestInEventSubject() },
                { NotificationEventType.EventParticipantAdded, new EventParticipantAddedSubject() },
                { NotificationEventType.EventParticipantRemoved, new EventParticipantRemovedSubject() },
                { NotificationEventType.PostCreated, new PostCreatedSubject() },
                { NotificationEventType.MentionedInPost, new MentionedInPostSubject() },
                { NotificationEventType.EventReminder, new EventReminderSubject() },
                { NotificationEventType.PasswordResetRequest, new PasswordResetRequestSubject() },
                { NotificationEventType.Other, new OtherSubject() }
            };

        public static string CreateSubject(NotificationEventType eventType, string details)
        {
            if (Strategies.TryGetValue(eventType, out var strategy))
            {
                return strategy.GenerateSubject(details);
            }

            return "You have a new notification!"; 
        }
    }
}
