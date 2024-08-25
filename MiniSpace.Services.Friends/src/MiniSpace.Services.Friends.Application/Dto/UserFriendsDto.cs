using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Dto
{
    public class UserFriendsDto
    {
        public Guid UserId { get; set; }
        public List<FriendDto> Friends { get; set; } = new List<FriendDto>();
    }
}
