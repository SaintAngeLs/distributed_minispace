using Paralax.CQRS.Queries;
using MiniSpace.Services.Email.Application.Dto;
using MiniSpace.Services.Email.Core.Entities;
using System;

namespace MiniSpace.Services.Email.Application.Queries
{
    public class GetEmailNotification : IQuery<EmailNotification>
    {
        public Guid UserId { get; set; }
        public Guid EmailId { get; set; }

        public GetEmailNotification(Guid userId, Guid notificationId)
        {
            UserId = userId;
            EmailId = notificationId;
        }
    }
}
