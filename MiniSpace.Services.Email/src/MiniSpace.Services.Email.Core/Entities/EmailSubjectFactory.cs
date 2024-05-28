namespace MiniSpace.Services.Email.Core.Entities
{
    public class EmailSubjectFactory
    {
        public static string CreateSubject(NotificationEventType eventType, string details)
        {
            return eventType switch
            {
                NotificationEventType.NewFriendRequest => "You have a new friend request!",
                NotificationEventType.NewPost => "Check out new posts in your feed!",
                NotificationEventType.NewEvent => "New event alert: " + details,
                NotificationEventType.FriendRequestAccepted => "Your friend request has been accepted!",
                NotificationEventType.MentionedInPost => "You've been mentioned in a post!",
                NotificationEventType.EventReminder => "Reminder for your upcoming event!",
                NotificationEventType.Other => "Notification: " + details,
                _ => "You have a new notification!"
            };
        }
    }
}