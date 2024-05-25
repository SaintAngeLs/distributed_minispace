using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.DTO
{
    public class StudentFriendsDto
    {
        public Guid StudentId { get; set; }
        public List<FriendDto> Friends { get; set; } = new List<FriendDto>();
    }
}