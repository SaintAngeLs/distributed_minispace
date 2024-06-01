using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class StudentFriendsDto
    {
        public Guid StudentId { get; set; }
        public List<FriendDto> Friends { get; set; } = new List<FriendDto>();
    }
}