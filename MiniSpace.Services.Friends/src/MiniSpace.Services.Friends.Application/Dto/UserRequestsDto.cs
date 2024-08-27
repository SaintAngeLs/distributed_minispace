using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Dto
{
    public class UserRequestsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<FriendRequestDto> FriendRequests { get; set; } = new List<FriendRequestDto>();
    }
}
