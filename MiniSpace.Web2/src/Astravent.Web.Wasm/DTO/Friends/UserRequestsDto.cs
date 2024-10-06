using System;
using System.Collections.Generic;

namespace  Astravent.Web.Wasm.DTO.Friends
{
    public class UserRequestsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<FriendRequestDto> FriendRequests { get; set; } = new List<FriendRequestDto>();
    }
}
