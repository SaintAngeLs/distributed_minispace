using Convey.CQRS.Queries;
using MiniSpace.Services.Email.Application.Dto;
using System;

namespace MiniSpace.Services.Email.Application.Queries
{
    public class GetEmail : IQuery<EmailDto>
    {
        public Guid UserId { get; set; }
        public Guid EmailId { get; set; }

        public GetEmail(Guid userId, Guid notificationId)
        {
            UserId = userId;
            EmailId = notificationId;
        }
    }
}
