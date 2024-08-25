using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetIncomingFriendRequests : IQuery<IEnumerable<UserRequestsDto>>
    {
        public Guid UserId { get; set; }
    }
}
