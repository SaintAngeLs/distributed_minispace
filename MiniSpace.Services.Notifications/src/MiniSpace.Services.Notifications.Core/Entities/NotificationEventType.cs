namespace MiniSpace.Services.Notifications.Core.Entities
{
    public enum NotificationEventType
    {
        NewFriendRequest,
        NewPost,
        NewEvent,
        EventDeleted,
        FriendRequestAccepted,
        MentionedInPost,
        EventReminder,
        Other
    }
}