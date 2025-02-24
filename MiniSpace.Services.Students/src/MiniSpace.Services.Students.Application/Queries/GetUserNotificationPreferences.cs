using Paralax.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using System;

namespace MiniSpace.Services.Students.Application.Queries
{
    public class GetUserNotificationPreferences : IQuery<NotificationPreferencesDto>
    {
        public Guid UserId { get; }

        public GetUserNotificationPreferences(Guid userId)
        {
            UserId = userId;
        }
    }
}
