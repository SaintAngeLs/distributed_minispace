using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Events.Application.DTO
{
    public class StudentFriendsDto
    {
        public Guid StudentId { get; set; }
        public List<FriendDto> Friends { get; set; } = new List<FriendDto>();
    }
}