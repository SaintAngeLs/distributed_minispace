using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetFriend : IQuery<FriendDto>
    {
        public Guid UserId { get; set; }
    }    
}
