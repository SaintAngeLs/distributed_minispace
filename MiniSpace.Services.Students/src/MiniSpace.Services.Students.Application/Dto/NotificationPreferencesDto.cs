using System;

namespace MiniSpace.Services.Students.Application.Dto
{
    public class NotificationPreferencesDto
    {
        public Guid StudentId { get; set; }
        public bool AccountChanges { get; set; }
        public bool SystemLogin { get; set; }
        public bool NewEvent { get; set; }
        public bool InterestBasedEvents { get; set; }
        public bool EventNotifications { get; set; }
        public bool CommentsNotifications { get; set; }
        public bool PostsNotifications { get; set; }
        public bool FriendsNotifications { get; set; }

        public NotificationPreferencesDto()
        {
            AccountChanges = false;
            SystemLogin = false;
            NewEvent = false;
            InterestBasedEvents = false;
            EventNotifications = false;
            CommentsNotifications = false;
            PostsNotifications = false;
            FriendsNotifications = false;
        }

        public NotificationPreferencesDto(Guid studentId, bool accountChanges, bool systemLogin, bool newEvent, bool interestBasedEvents,
                                          bool eventNotifications, bool commentsNotifications, bool postsNotifications,
                                          bool friendsNotifications)
        {
            StudentId = studentId;
            AccountChanges = accountChanges;
            SystemLogin = systemLogin;
            NewEvent = newEvent;
            InterestBasedEvents = interestBasedEvents;
            EventNotifications = eventNotifications;
            CommentsNotifications = commentsNotifications;
            PostsNotifications = postsNotifications;
            FriendsNotifications = friendsNotifications;
        }
    }
}
