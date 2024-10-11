using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateUserNotificationPreferences : ICommand
    {
        public Guid UserId { get; set; }
        public bool EmailNotifications { get; set; }
        
        public bool SystemLogin { get; set; }
        public bool InterestBasedEvents { get; set; }
        public bool EventNotifications { get; set; }
        public bool CommentsNotifications { get; set; }
        public bool PostsNotifications { get; set; }

        public bool EventRecommendation { get; set; }
        public bool FriendsRecommendation { get; set; }
        public bool FriendsPosts { get; set; }
        public bool PostsRecommendation { get; set; }
        public bool EventsIAmInterestedInNotification { get; set; }
        public bool EventsIAmSignedUpToNotification { get; set; }
        public bool PostsOfPeopleIFollowNotification { get; set; }
        public bool EventNotificationForPeopleIFollow { get; set; }

        // New properties for friend requests and post notifications
        public bool NewFriendsRequests { get; set; }
        public bool MyRequestsAccepted { get; set; }
        public bool FriendsPostsNotifications { get; set; }

        public UpdateUserNotificationPreferences(Guid userId, bool emailNotifications, bool systemLogin, bool interestBasedEvents, 
            bool eventNotifications, bool commentsNotifications, bool postsNotifications, bool eventRecommendation, 
            bool friendsRecommendation, bool friendsPosts, bool postsRecommendation, bool eventsIAmInterestedInNotification, 
            bool eventsIAmSignedUpToNotification, bool postsOfPeopleIFollowNotification, bool eventNotificationForPeopleIFollow,
            bool newFriendsRequests, bool myRequestsAccepted, bool friendsPostsNotifications)
        {
            UserId = userId;
            EmailNotifications = emailNotifications;
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
