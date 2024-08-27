using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Application.Dto
{
    public interface UserFriendsDto
    {
        public Guid StudentId { get; set; }
        public List<FriendDto> Friends { get; set; }
    }
}