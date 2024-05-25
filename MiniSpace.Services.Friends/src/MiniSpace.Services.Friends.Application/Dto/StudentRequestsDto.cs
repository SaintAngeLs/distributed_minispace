using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Dto
{
    public class StudentRequestsDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public List<FriendRequestDto> FriendRequests { get; set; } = new List<FriendRequestDto>();
    }
}
