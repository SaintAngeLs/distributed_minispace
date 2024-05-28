namespace MiniSpace.Services.Notifications.Core.Entities
{
    public enum NotificationEventType
    {
        NewFriendRequest,
        FriendRequestAccepted,
        NewEvent,
        EventDeleted,
        EventNewSignUp,
        EventNewSignUpFriend,
        StudentCancelledSignedUpToEvent,
        StudentShowedInterestInEvent,
        EventParticipantAdded,
        NewPost,
        MentionedInPost,
        EventReminder,
        Other
    }
}