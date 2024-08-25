using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using System;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetSentFriendRequests : IQuery<IEnumerable<UserRequestsDto>>
    {
        public Guid UserId { get; set; }
    }
}
