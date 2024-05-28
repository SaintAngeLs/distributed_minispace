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
        StudentShowedInterestInEvent,
        FriendRequestAccepted,
        MentionedInPost,
        EventReminder,
        Other
    }
}