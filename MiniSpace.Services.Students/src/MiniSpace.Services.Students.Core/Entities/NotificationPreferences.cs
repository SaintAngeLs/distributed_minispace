using System;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class NotificationPreferences
    {
        public bool SystemLogin { get; private set; }
        public bool InterestBasedEvents { get; private set; }
        public bool EventNotifications { get; private set; }
        public bool CommentsNotifications { get; private set; }
        public bool PostsNotifications { get; private set; }

        public bool EventRecommendation { get; private set; }
        public bool FriendsRecommendation { get; private set; }
        public bool FriendsPosts { get; private set; }
        public bool PostsRecommendation { get; private set; }
        public bool EventsIAmInterestedInNotification { get; private set; }
        public bool EventsIAmSignedUpToNotification { get; private set; }
        public bool PostsOfPeopleIFollowNotification { get; private set; }
        public bool EventNotificationForPeopleIFollow { get; private set; }

        // New properties for notifications
        public bool NewFriendsRequests { get; private set; }
        public bool MyRequestsAccepted { get; private set; }
        public bool FriendsPostsNotifications { get; private set; }

        public NotificationPreferences()
        {
            SystemLogin = false;
            InterestBasedEvents = false;
            EventNotifications = false;
            CommentsNotifications = false;
            PostsNotifications = false;

            EventRecommendation = false;
            FriendsRecommendation = false;
            FriendsPosts = false;
            PostsRecommendation = false;
            EventsIAmInterestedInNotification = false;
            EventsIAmSignedUpToNotification = false;
            PostsOfPeopleIFollowNotification = false;
            EventNotificationForPeopleIFollow = false;

            NewFriendsRequests = false;
            MyRequestsAccepted = false;
            FriendsPostsNotifications = false;
        }

        public NotificationPreferences(bool systemLogin, bool interestBasedEvents, bool eventNotifications,
                                       bool commentsNotifications, bool postsNotifications, bool eventRecommendation,
                                       bool friendsRecommendation, bool friendsPosts, bool postsRecommendation,
                                       bool eventsIAmInterestedInNotification, bool eventsIAmSignedUpToNotification,
                                       bool postsOfPeopleIFollowNotification, bool eventNotificationForPeopleIFollow,
                                       bool newFriendsRequests, bool myRequestsAccepted, bool friendsPostsNotifications)
        {
            SystemLogin = systemLogin;
            InterestBasedEvents = interestBasedEvents;
            EventNotifications = eventNotifications;
            CommentsNotifications = commentsNotifications;
            PostsNotifications = postsNotifications;

            EventRecommendation = eventRecommendation;
            FriendsRecommendation = friendsRecommendation;
            FriendsPosts = friendsPosts;
            PostsRecommendation = postsRecommendation;
            EventsIAmInterestedInNotification = eventsIAmInterestedInNotification;
            EventsIAmSignedUpToNotification = eventsIAmSignedUpToNotification;
            PostsOfPeopleIFollowNotification = postsOfPeopleIFollowNotification;
            EventNotificationForPeopleIFollow = eventNotificationForPeopleIFollow;

            NewFriendsRequests = newFriendsRequests;
            MyRequestsAccepted = myRequestsAccepted;
            FriendsPostsNotifications = friendsPostsNotifications;
        }

        public void UpdatePreferences(bool systemLogin, bool interestBasedEvents, bool eventNotifications,
                                      bool commentsNotifications, bool postsNotifications, bool eventRecommendation,
                                      bool friendsRecommendation, bool friendsPosts, bool postsRecommendation,
                                      bool eventsIAmInterestedInNotification, bool eventsIAmSignedUpToNotification,
                                      bool postsOfPeopleIFollowNotification, bool eventNotificationForPeopleIFollow,
                                      bool newFriendsRequests, bool myRequestsAccepted, bool friendsPostsNotifications)
        {
            SystemLogin = systemLogin;
            InterestBasedEvents = interestBasedEvents;
            EventNotifications = eventNotifications;
            CommentsNotifications = commentsNotifications;
            PostsNotifications = postsNotifications;

            EventRecommendation = eventRecommendation;
            FriendsRecommendation = friendsRecommendation;
            FriendsPosts = friendsPosts;
            PostsRecommendation = postsRecommendation;
            EventsIAmInterestedInNotification = eventsIAmInterestedInNotification;
            EventsIAmSignedUpToNotification = eventsIAmSignedUpToNotification;
            PostsOfPeopleIFollowNotification = postsOfPeopleIFollowNotification;
            EventNotificationForPeopleIFollow = eventNotificationForPeopleIFollow;

            NewFriendsRequests = newFriendsRequests;
            MyRequestsAccepted = myRequestsAccepted;
            FriendsPostsNotifications = friendsPostsNotifications;
        }
    }
}
