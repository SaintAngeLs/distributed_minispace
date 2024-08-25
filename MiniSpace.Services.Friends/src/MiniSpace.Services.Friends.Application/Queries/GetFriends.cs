using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetFriends : IQuery<IEnumerable<UserFriendsDto>>
    {
        public Guid UserId { get; set; }
    }    
}