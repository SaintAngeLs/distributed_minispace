using System;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class NotificationPreferences
    {
        public bool AccountChanges { get; private set; }
        public bool SystemLogin { get; private set; }
        public bool NewEvent { get; private set; }
        public bool InterestBasedEvents { get; private set; }
        public bool EventNotifications { get; private set; }
        public bool CommentsNotifications { get; private set; }
        public bool PostsNotifications { get; private set; }
        public bool FriendsNotifications { get; private set; }

         
        public NotificationPreferences()
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


        public NotificationPreferences(bool accountChanges, bool systemLogin, bool newEvent, bool interestBasedEvents,
                                       bool eventNotifications, bool commentsNotifications, bool postsNotifications,
                                       bool friendsNotifications)
        {
            AccountChanges = accountChanges;
            SystemLogin = systemLogin;
            NewEvent = newEvent;
            InterestBasedEvents = interestBasedEvents;
            EventNotifications = eventNotifications;
            CommentsNotifications = commentsNotifications;
            PostsNotifications = postsNotifications;
            FriendsNotifications = friendsNotifications;
        }

        public void UpdatePreferences(bool accountChanges, bool systemLogin, bool newEvent, bool interestBasedEvents,
                                      bool eventNotifications, bool commentsNotifications, bool postsNotifications,
                                      bool friendsNotifications)
        {
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
