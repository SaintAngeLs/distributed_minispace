using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetFriendRequests : IQuery<IEnumerable<FriendRequestDto>>
    {
        public Guid StudentId { get; set; }

        public GetFriendRequests(Guid userId)
        {
            StudentId = userId;
        }
    }
}
