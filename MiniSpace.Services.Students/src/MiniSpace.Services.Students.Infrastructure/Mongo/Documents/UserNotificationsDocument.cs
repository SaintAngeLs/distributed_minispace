using Convey.Types;
using MiniSpace.Services.Students.Core.Entities;
using System;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public class UserNotificationsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public NotificationPreferences NotificationPreferences { get; set; }
    }
}
