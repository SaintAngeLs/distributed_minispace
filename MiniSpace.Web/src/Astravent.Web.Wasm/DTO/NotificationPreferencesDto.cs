using System;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO
{
    public class NotificationPreferencesDto
    {
        public Guid UserId { get; set; }
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

        public bool NewFriendsRequests { get; set; }
        public bool MyRequestsAccepted { get; set; }
        public bool FriendsPostsNotifications { get; set; }

    }
}
