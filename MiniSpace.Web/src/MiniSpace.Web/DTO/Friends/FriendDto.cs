using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.States;

namespace MiniSpace.Web.DTO.Friends
{
    public class FriendDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public DateTime CreatedAt { get; set; }
        public FriendState State { get; set; }
        public StudentDto StudentDetails { get; set; }
    }
}
