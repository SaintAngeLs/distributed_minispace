using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class UserFriendsDto
    {
        public Guid UserId { get; set; }
        public List<FriendDto> Friends { get; set; } = new List<FriendDto>();
    }
}