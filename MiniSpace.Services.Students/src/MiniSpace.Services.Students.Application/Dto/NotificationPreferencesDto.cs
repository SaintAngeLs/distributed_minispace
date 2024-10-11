using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class NotificationPreferencesDto
    {
        public Guid StudentId { get; set; }
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

        public NotificationPreferencesDto()
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
        }

        public NotificationPreferencesDto(Guid studentId, bool systemLogin, bool interestBasedEvents, bool eventNotifications,
                                          bool commentsNotifications, bool postsNotifications, bool eventRecommendation,
                                          bool friendsRecommendation, bool friendsPosts, bool postsRecommendation,
                                          bool eventsIAmInterestedInNotification, bool eventsIAmSignedUpToNotification,
                                          bool postsOfPeopleIFollowNotification, bool eventNotificationForPeopleIFollow)
        {
            StudentId = studentId;
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
        }
    }
}
