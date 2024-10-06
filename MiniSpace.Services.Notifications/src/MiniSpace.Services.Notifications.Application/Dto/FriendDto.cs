using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class FriendDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string FriendState { get; set; }
    }
}