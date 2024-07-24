using Convey.Types;
using MiniSpace.Services.Students.Core.Entities;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class UserNotificationsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public NotificationPreferences NotificationPreferences { get; set; }
    }
}
