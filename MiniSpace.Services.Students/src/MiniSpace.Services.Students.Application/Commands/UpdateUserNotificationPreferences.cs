using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateUserNotificationPreferences : ICommand
    {
        public Guid StudentId { get; set; }
        public bool EmailNotifications { get; set; }
        public bool AccountChanges { get; set; }
        public bool SystemLogin { get; set; }
        public bool NewEvent { get; set; }
        public bool InterestBasedEvents { get; set; }
        public bool EventNotifications { get; set; }
        public bool CommentsNotifications { get; set; }
        public bool PostsNotifications { get; set; }
        public bool FriendsNotifications { get; set; }
    }

}
