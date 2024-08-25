using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.DTO.Friends
{
    public class UserFriendsDto
    {
        public Guid UserId { get; set; }
        public List<FriendDto> Friends { get; set; } = new List<FriendDto>();
    }
}