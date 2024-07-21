using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateUserNotificationPreferences : ICommand
    {
        public Guid StudentId { get; }
        public bool AccountChanges { get; }
        public bool SystemLogin { get; }
        public bool NewEvent { get; }
        public bool InterestBasedEvents { get; }
        public bool EventNotifications { get; }
        public bool CommentsNotifications { get; }
        public bool PostsNotifications { get; }
        public bool FriendsNotifications { get; }

        public UpdateUserNotificationPreferences(Guid studentId, bool accountChanges, bool systemLogin, bool newEvent,
                                                 bool interestBasedEvents, bool eventNotifications,
                                                 bool commentsNotifications, bool postsNotifications,
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
