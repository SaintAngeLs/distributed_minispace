namespace MiniSpace.Services.Notifications.Core.Entities
{
    public enum NotificationEventType
    {
        NewFriendRequest,
        NewPost,
        NewEvent,
        EventDeleted,
        EventNewSignUp,
        EventNewSignUpFriend,
        StudentCancelledSignedUpToEvent,
        FriendRequestAccepted,
        MentionedInPost,
        EventReminder,
        Other
    }
}